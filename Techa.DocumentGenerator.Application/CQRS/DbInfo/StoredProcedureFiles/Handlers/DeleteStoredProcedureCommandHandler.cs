using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Commands;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Handlers
{
    public class DeleteStoredProcedureCommandHandler : IRequestHandler<DeleteStoredProcedureCommand, HandlerResponse<bool>>
    {
        private readonly IBaseService<StoredProcedure> _service;

        public DeleteStoredProcedureCommandHandler(IBaseService<StoredProcedure> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<bool>> Handle(DeleteStoredProcedureCommand request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Id);

            if (obj == null)
                return new(false, "رکورد موردنظر یافت نشد", false);

            await _service.DeleteAsync(obj, cancellationToken);
            return true;
        }
    }
}
