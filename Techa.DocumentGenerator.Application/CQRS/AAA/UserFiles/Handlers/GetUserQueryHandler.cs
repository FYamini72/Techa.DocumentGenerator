using Techa.DocumentGenerator.Application.Dtos.AAA;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Queries;
using Techa.DocumentGenerator.Application.Services.Interfaces.AAA;
using Microsoft.EntityFrameworkCore;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Handlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, HandlerResponse<UserDisplayDto>>
    {
        private readonly IUserService _userService;

        public GetUserQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<HandlerResponse<UserDisplayDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetAll(x => x.Id == request.Id)
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .Include(x => x.Profile)
                .FirstOrDefaultAsync();

            if (user == null)
                return new(false, "کاربر موردنظر یافت نشد", null);

            return user.Adapt<UserDisplayDto>();
        }
    }
}
