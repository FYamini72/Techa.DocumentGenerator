using MediatR;
using Techa.DocumentGenerator.Application.Dtos.BaseInfo;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Commands
{
    public class UpdateProjectCommand : IRequest<HandlerResponse<ProjectDisplayDto>>
    {
        public ProjectCreateDto Project { get; }

        public UpdateProjectCommand(ProjectCreateDto Project)
        {
            this.Project = Project;
        }
    }
}