using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Commands;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Handlers
{
    public class UpdateStoredProcedureParameterCommandHandler : IRequestHandler<UpdateStoredProcedureParameterCommand, HandlerResponse<StoredProcedureParameterDisplayDto>>
    {
        private readonly IBaseService<StoredProcedureParameter> _service;

        public UpdateStoredProcedureParameterCommandHandler(IBaseService<StoredProcedureParameter> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<StoredProcedureParameterDisplayDto>> Handle(UpdateStoredProcedureParameterCommand request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.StoredProcedureParameter.Id);

            if (obj == null)
                return new(false, "رکورد مورد نظر یافت نشد", null);

            request.StoredProcedureParameter.Adapt(obj);
            var result = await _service.UpdateAsync(obj, cancellationToken);
            return result.Adapt<StoredProcedureParameterDisplayDto>();
        }
    }
}
