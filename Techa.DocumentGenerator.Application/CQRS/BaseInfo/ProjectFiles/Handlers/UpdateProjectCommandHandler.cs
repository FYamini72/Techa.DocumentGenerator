using Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Commands;
using Techa.DocumentGenerator.Application.Dtos.BaseInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Handlers
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, HandlerResponse<ProjectDisplayDto>>
    {
        private readonly IBaseService<Project> _service;

        public UpdateProjectCommandHandler(IBaseService<Project> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<ProjectDisplayDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Project.Id);

            if (obj == null)
                return new(false, "رکورد مورد نظر یافت نشد", null);

            request.Project.Adapt(obj);
            var result = await _service.UpdateAsync(obj, cancellationToken);
            return result.Adapt<ProjectDisplayDto>();
        }
    }
}
