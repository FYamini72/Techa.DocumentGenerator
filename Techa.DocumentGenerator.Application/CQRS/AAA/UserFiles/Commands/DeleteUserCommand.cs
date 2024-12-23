using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Commands
{
    public class DeleteUserCommand : IRequest<HandlerResponse<bool>>
    {
        public int Id { get; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
