using MediatR;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries
{
    public class GetStoredProcedureQuery : IRequest<HandlerResponse<StoredProcedureDisplayDto>>
    {
        public int Id { get; }

        public GetStoredProcedureQuery(int id)
        {
            Id = id;
        }
    }
}