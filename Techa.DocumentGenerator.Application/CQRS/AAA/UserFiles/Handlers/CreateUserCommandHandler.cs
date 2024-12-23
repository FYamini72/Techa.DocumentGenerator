using Techa.DocumentGenerator.Application.Dtos.AAA;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Utilities;
using Techa.DocumentGenerator.Domain.Entities.AAA;
using Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Commands;
using Techa.DocumentGenerator.Application.Services.Interfaces.AAA;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, HandlerResponse<UserDisplayDto>>
    {
        private readonly IUserService _userService;
        private readonly IBaseService<Role> _roleService;
        private readonly IAttachmentFileService _attachmentFileService;

        public CreateUserCommandHandler (IUserService userService
            , IBaseService<Role> roleService
            , IAttachmentFileService attachmentFileService)
        {
            _userService = userService;
            _roleService = roleService;
            _attachmentFileService = attachmentFileService;
        }

        public async Task<HandlerResponse<UserDisplayDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.User.Adapt<User>();

            if (request.User.SelectedFile != null)
                user.Profile = await _attachmentFileService.UploadFile(request.User.SelectedFile, cancellationToken);

            var role = await _roleService.FirstOrDefaultAsync(cancellationToken, x => x.Title == "Student");

            if (role != null) 
            {
                user.UserRoles = new List<UserRole>()
                {
                    new UserRole()
                    {
                        RoleId = role.Id
                    }
                };
            }

            user.PasswordHash = request.User.Password.GetSha256Hash();
            user.SecurityStamp = Guid.NewGuid();

            var result = await _userService.AddAsync(user, cancellationToken);

            if (result.ProfileId.HasValue && result.Profile == null)
                result.Profile = await _attachmentFileService.GetByIdAsync(cancellationToken, result.ProfileId);
            
            return result.Adapt<UserDisplayDto>();
        }
    }
}
