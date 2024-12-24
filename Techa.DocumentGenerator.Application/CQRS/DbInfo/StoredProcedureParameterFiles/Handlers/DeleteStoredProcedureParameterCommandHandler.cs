using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Commands;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Handlers
{
    public class DeleteStoredProcedureParameterCommandHandler : IRequestHandler<DeleteStoredProcedureParameterCommand, HandlerResponse<bool>>
    {
        private readonly IBaseService<StoredProcedureParameter> _service;

        public DeleteStoredProcedureParameterCommandHandler(IBaseService<StoredProcedureParameter> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<bool>> Handle(DeleteStoredProcedureParameterCommand request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Id);

            if (obj == null)
                return new(false, "رکورد موردنظر یافت نشد", false);

            await _service.DeleteAsync(obj, cancellationToken);
            return true;
        }
    }
}
