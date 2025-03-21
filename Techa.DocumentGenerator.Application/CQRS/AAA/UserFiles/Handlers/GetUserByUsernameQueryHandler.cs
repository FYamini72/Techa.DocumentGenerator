using Techa.DocumentGenerator.Application.Dtos.AAA;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Queries;
using Techa.DocumentGenerator.Application.Services.Interfaces.AAA;
using Microsoft.EntityFrameworkCore;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Handlers
{
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, HandlerResponse<UserDisplayDto>>
    {
        private readonly IUserService _userService;

        public GetUserByUsernameQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<HandlerResponse<UserDisplayDto>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService
                .GetAll(user => user.UserName == request.Username && user.ProjectId == request.ProjectId)
                .FirstOrDefaultAsync();

            if (user == null)
                return new(false, "کاربر موردنظر یافت نشد", null);

            return user.Adapt<UserDisplayDto>();
        }
    }
}
