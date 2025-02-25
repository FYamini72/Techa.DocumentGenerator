using MediatR;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries
{
    public class GetStoredProcedureScriptQuery : IRequest<HandlerResponse<StoredProcedureScriptDto>>
    {
        public int Id { get; }

        public GetStoredProcedureScriptQuery(int id)
        {
            Id = id;
        }
    }
}