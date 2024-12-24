using MediatR;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Queries
{
    public class GetStoredProcedureParameterQuery : IRequest<HandlerResponse<StoredProcedureParameterDisplayDto>>
    {
        public int Id { get; }

        public GetStoredProcedureParameterQuery(int id)
        {
            Id = id;
        }
    }
}