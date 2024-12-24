using MediatR;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Commands
{
    public class UpdateStoredProcedureParameterCommand : IRequest<HandlerResponse<StoredProcedureParameterDisplayDto>>
    {
        public StoredProcedureParameterCreateDto StoredProcedureParameter { get; }

        public UpdateStoredProcedureParameterCommand(StoredProcedureParameterCreateDto StoredProcedureParameter)
        {
            this.StoredProcedureParameter = StoredProcedureParameter;
        }
    }
}