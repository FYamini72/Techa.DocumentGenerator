using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Handlers
{
    public class GetStoredProcedureQueryHandler : IRequestHandler<GetStoredProcedureQuery, HandlerResponse<StoredProcedureDisplayDto>>
    {
        private readonly IBaseService<StoredProcedure> _service;

        public GetStoredProcedureQueryHandler(IBaseService<StoredProcedure> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<StoredProcedureDisplayDto>> Handle(GetStoredProcedureQuery request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Id);

            if (obj == null)
                return new(false, "رکورد موردنظر یافت نشد", null);

            return obj.Adapt<StoredProcedureDisplayDto>();
        }
    }
}