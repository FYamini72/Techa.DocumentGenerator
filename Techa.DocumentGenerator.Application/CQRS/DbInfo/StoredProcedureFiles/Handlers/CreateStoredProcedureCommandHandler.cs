using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Commands;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Handlers
{
    public class CreateStoredProcedureCommandHandler : IRequestHandler<CreateStoredProcedureCommand, HandlerResponse<StoredProcedureDisplayDto>>
    {
        private readonly IBaseService<StoredProcedure> _service;

        public CreateStoredProcedureCommandHandler(IBaseService<StoredProcedure> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<StoredProcedureDisplayDto>> Handle(CreateStoredProcedureCommand request, CancellationToken cancellationToken)
        {
            var StoredProcedure = request.StoredProcedure.Adapt<StoredProcedure>();

            var result = await _service.AddAsync(StoredProcedure, cancellationToken);
            return result.Adapt<StoredProcedureDisplayDto>();
        }
    }
}
