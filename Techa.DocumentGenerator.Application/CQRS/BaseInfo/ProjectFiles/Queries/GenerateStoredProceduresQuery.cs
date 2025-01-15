using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Queries
{
    public class GenerateStoredProceduresQuery : IRequest<HandlerResponse>
    {
        public int Id { get; }

        public GenerateStoredProceduresQuery(int id)
        {
            Id = id;
        }
    }
}