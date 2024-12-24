using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Handlers
{
    public class GetStoredProcedureParameterQueryHandler : IRequestHandler<GetStoredProcedureParameterQuery, HandlerResponse<StoredProcedureParameterDisplayDto>>
    {
        private readonly IBaseService<StoredProcedureParameter> _service;

        public GetStoredProcedureParameterQueryHandler(IBaseService<StoredProcedureParameter> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<StoredProcedureParameterDisplayDto>> Handle(GetStoredProcedureParameterQuery request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Id);

            if (obj == null)
                return new(false, "رکورد موردنظر یافت نشد", null);

            return obj.Adapt<StoredProcedureParameterDisplayDto>();
        }
    }
}