using Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.BaseInfo;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Handlers
{
    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, HandlerResponse<ProjectDisplayDto>>
    {
        private readonly IBaseService<Project> _service;

        public GetProjectQueryHandler(IBaseService<Project> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<ProjectDisplayDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Id);

            if (obj == null)
                return new(false, "رکورد موردنظر یافت نشد", null);

            return obj.Adapt<ProjectDisplayDto>();
        }
    }
}