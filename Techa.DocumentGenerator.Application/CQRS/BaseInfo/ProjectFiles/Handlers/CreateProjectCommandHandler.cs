using Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Commands;
using Techa.DocumentGenerator.Application.Dtos.BaseInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Handlers
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, HandlerResponse<ProjectDisplayDto>>
    {
        private readonly IBaseService<Project> _service;

        public CreateProjectCommandHandler(IBaseService<Project> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<ProjectDisplayDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var Project = request.Project.Adapt<Project>();

            var result = await _service.AddAsync(Project, cancellationToken);
            return result.Adapt<ProjectDisplayDto>();
        }
    }
}
