using MediatR;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Commands
{
    public class CreateStoredProcedureParameterCommand : IRequest<HandlerResponse<StoredProcedureParameterDisplayDto>>
    {
        public StoredProcedureParameterCreateDto StoredProcedureParameter { get; }

        public CreateStoredProcedureParameterCommand(StoredProcedureParameterCreateDto StoredProcedureParameter)
        {
            this.StoredProcedureParameter = StoredProcedureParameter;
        }
    }
}