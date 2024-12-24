using MediatR;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Commands
{
    public class UpdateStoredProcedureCommand : IRequest<HandlerResponse<StoredProcedureDisplayDto>>
    {
        public StoredProcedureCreateDto StoredProcedure { get; }

        public UpdateStoredProcedureCommand(StoredProcedureCreateDto StoredProcedure)
        {
            this.StoredProcedure = StoredProcedure;
        }
    }
}