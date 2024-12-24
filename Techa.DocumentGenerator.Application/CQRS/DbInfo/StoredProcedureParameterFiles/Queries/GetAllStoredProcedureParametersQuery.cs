using MediatR;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Queries
{
    public class GetAllStoredProcedureParametersQuery : IRequest<HandlerResponse<List<StoredProcedureParameterDisplayDto>>>
    {
        public StoredProcedureParameterSearchDto? SearchDto { get; }

        public GetAllStoredProcedureParametersQuery(StoredProcedureParameterSearchDto? searchDto)
        {
            this.SearchDto = searchDto;
        }
    }
}