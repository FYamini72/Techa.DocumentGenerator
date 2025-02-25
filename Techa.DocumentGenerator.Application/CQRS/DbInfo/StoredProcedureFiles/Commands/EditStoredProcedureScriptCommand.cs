using MediatR;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Commands
{
    public class EditStoredProcedureScriptCommand : IRequest<HandlerResponse>
    {
        public StoredProcedureScriptDto StoredProcedure { get; }

        public EditStoredProcedureScriptCommand(StoredProcedureScriptDto StoredProcedure)
        {
            this.StoredProcedure = StoredProcedure;
        }
    }
}