using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Commands;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Handlers
{
    public class UpdateStoredProcedureCommandHandler : IRequestHandler<UpdateStoredProcedureCommand, HandlerResponse<StoredProcedureDisplayDto>>
    {
        private readonly IBaseService<StoredProcedure> _service;

        public UpdateStoredProcedureCommandHandler(IBaseService<StoredProcedure> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<StoredProcedureDisplayDto>> Handle(UpdateStoredProcedureCommand request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.StoredProcedure.Id);

            if (obj == null)
                return new(false, "رکورد مورد نظر یافت نشد", null);

            request.StoredProcedure.Adapt(obj);
            var result = await _service.UpdateAsync(obj, cancellationToken);
            return result.Adapt<StoredProcedureDisplayDto>();
        }
    }
}
