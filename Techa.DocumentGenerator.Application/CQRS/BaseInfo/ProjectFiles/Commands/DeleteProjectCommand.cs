using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Commands
{
    public class DeleteProjectCommand : IRequest<HandlerResponse<bool>>
    {
        public int Id { get; }

        public DeleteProjectCommand(int id)
        {
            Id = id;
        }
    }
}