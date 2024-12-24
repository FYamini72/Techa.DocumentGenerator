using MediatR;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Commands
{
    public class CreateStoredProcedureCommand : IRequest<HandlerResponse<StoredProcedureDisplayDto>>
    {
        public StoredProcedureCreateDto StoredProcedure { get; }

        public CreateStoredProcedureCommand(StoredProcedureCreateDto StoredProcedure)
        {
            this.StoredProcedure = StoredProcedure;
        }
    }
}