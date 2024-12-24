using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Commands
{
    public class DeleteStoredProcedureParameterCommand : IRequest<HandlerResponse<bool>>
    {
        public int Id { get; }

        public DeleteStoredProcedureParameterCommand(int id)
        {
            Id = id;
        }
    }
}