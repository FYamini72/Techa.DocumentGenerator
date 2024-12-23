using Techa.DocumentGenerator.Application.Dtos.AAA;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Commands
{
    public class UpdateUserCommand : IRequest<HandlerResponse<UserDisplayDto>>
    {
        public UserUpdateDto User { get; }

        public UpdateUserCommand(UserUpdateDto user)
        {
            User = user;
        }
    }
}
