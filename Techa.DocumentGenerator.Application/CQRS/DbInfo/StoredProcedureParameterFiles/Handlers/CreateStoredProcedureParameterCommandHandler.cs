using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Commands;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Handlers
{
    public class CreateStoredProcedureParameterCommandHandler : IRequestHandler<CreateStoredProcedureParameterCommand, HandlerResponse<StoredProcedureParameterDisplayDto>>
    {
        private readonly IBaseService<StoredProcedureParameter> _service;

        public CreateStoredProcedureParameterCommandHandler(IBaseService<StoredProcedureParameter> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<StoredProcedureParameterDisplayDto>> Handle(CreateStoredProcedureParameterCommand request, CancellationToken cancellationToken)
        {
            var StoredProcedureParameter = request.StoredProcedureParameter.Adapt<StoredProcedureParameter>();

            var result = await _service.AddAsync(StoredProcedureParameter, cancellationToken);
            return result.Adapt<StoredProcedureParameterDisplayDto>();
        }
    }
}
