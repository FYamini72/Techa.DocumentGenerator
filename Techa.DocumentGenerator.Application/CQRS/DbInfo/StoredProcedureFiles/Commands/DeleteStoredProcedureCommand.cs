using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Commands
{
    public class DeleteStoredProcedureCommand : IRequest<HandlerResponse<bool>>
    {
        public int Id { get; }

        public DeleteStoredProcedureCommand(int id)
        {
            Id = id;
        }
    }
}