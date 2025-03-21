using Techa.DocumentGenerator.Application.Dtos.AAA;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Utilities;
using Techa.DocumentGenerator.Domain.Entities.AAA;
using Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Commands;
using Techa.DocumentGenerator.Application.Services.Interfaces.AAA;
using Microsoft.EntityFrameworkCore;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, HandlerResponse<UserDisplayDto>>
    {
        private readonly IUserService _userService;
        private readonly IBaseService<Role> _roleService;
        private readonly IBaseService<UserRole> _userRoleService;
        private readonly IAttachmentFileService _attachmentFileService;

        public CreateUserCommandHandler(IUserService userService
            , IBaseService<Role> roleService
            , IBaseService<UserRole> userRoleService
            , IAttachmentFileService attachmentFileService)
        {
            _userService = userService;
            _roleService = roleService;
            _userRoleService = userRoleService;
            _attachmentFileService = attachmentFileService;
        }

        public async Task<HandlerResponse<UserDisplayDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.User.Adapt<User>();
            user.UserRoles = new List<UserRole>();

            if (request.User.SelectedFile != null)
                user.Profile = await _attachmentFileService.UploadFile(request.User.SelectedFile, cancellationToken);

            if (request.User.RoleIds != null && request.User.RoleIds.Any())
            {
                foreach (var roleId in request.User.RoleIds)
                {
                    var role = await _roleService.GetByIdAsync(cancellationToken, roleId);
                    if (role != null && role.ProjectId == user.ProjectId)
                    {
                        user.UserRoles.Add(new UserRole() { RoleId = roleId });
                    }
                    else
                    {
                        return new(false, "نقش های انتخاب شده برای کاربر نامعتبر است.", null);
                    }
                }
            }

            user.PasswordHash = request.User.Password.GetSha256Hash();
            user.SecurityStamp = Guid.NewGuid();

            var result = await _userService.AddAsync(user, cancellationToken);

            var userRoles = await _userRoleService
                .GetAll(x => x.UserId == result.Id)
                .Include(x => x.Role)
                .ToListAsync();
            if(userRoles.Any())
                result.UserRoles = userRoles;

            if (result.ProfileId.HasValue && result.Profile == null)
                result.Profile = await _attachmentFileService.GetByIdAsync(cancellationToken, result.ProfileId);
            
            return result.Adapt<UserDisplayDto>();
        }
    }
}
