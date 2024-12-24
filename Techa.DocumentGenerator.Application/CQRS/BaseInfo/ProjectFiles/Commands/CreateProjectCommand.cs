using MediatR;
using Techa.DocumentGenerator.Application.Dtos.BaseInfo;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Commands
{
    public class CreateProjectCommand : IRequest<HandlerResponse<ProjectDisplayDto>>
    {
        public ProjectCreateDto Project { get; }

        public CreateProjectCommand(ProjectCreateDto Project)
        {
            this.Project = Project;
        }
    }
}