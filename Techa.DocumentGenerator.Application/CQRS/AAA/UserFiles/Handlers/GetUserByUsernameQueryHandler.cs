using Techa.DocumentGenerator.Application.Dtos.AAA;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Queries;
using Techa.DocumentGenerator.Application.Services.Interfaces.AAA;

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
            var user = _userService.GetAll(user => user.UserName == request.Username);

            if (user == null)
                return new(false, "کاربر موردنظر یافت نشد", null);

            return user.FirstOrDefault().Adapt<UserDisplayDto>();
        }
    }
}
