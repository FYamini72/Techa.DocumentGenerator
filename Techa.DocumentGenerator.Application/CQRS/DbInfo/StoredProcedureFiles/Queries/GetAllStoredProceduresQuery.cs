using MediatR;
using Techa.DocumentGenerator.Application.Dtos;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries
{
    public class GetAllStoredProceduresQuery : IRequest<HandlerResponse<BaseGridDto<StoredProcedureDisplayDto>>>
    {
        public StoredProcedureSearchDto? SearchDto { get; }

        public GetAllStoredProceduresQuery(StoredProcedureSearchDto? searchDto)
        {
            this.SearchDto = searchDto;
        }
    }
}