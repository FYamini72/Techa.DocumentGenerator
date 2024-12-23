using MediatR;
using Techa.DocumentGenerator.Application.Dtos;

namespace Techa.DocumentGenerator.Application.CQRS.EventLogFiles.Queries
{
    public class GetAllEventLogsQuery : IRequest<HandlerResponse<List<EventLogDisplayDto>>>
    {
        public EventLogSearchDto? SearchDto { get; }

        public GetAllEventLogsQuery(EventLogSearchDto? searchDto)
        {
            this.SearchDto = searchDto;
        }
    }
}
