using MediatR;
using Techa.DocumentGenerator.Application.Dtos.AAA;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Commands
{
    public class CreateUserCommand : IRequest<HandlerResponse<UserDisplayDto>>
    {
        public UserCreateDto User { get; }

        public CreateUserCommand(UserCreateDto user)
        {
            User = user;
        }
    }
}