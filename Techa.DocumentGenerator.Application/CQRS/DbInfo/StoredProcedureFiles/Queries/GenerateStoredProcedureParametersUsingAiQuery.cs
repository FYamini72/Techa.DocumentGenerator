using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries
{
    public class GenerateStoredProcedureParametersUsingAiQuery : IRequest<HandlerResponse>
    {
        public int Id { get; }

        public GenerateStoredProcedureParametersUsingAiQuery(int id)
        {
            Id = id;
        }
    }
}