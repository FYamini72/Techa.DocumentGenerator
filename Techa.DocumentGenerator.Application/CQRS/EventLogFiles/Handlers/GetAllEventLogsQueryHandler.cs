using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.CQRS.EventLogFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities;

namespace Techa.DocumentGenerator.Application.CQRS.EventLogFiles.Handlers
{
    public class GetAllEventLogsQueryHandler : IRequestHandler<GetAllEventLogsQuery, HandlerResponse<List<EventLogDisplayDto>>>
    {
        private readonly IBaseService<EventLog> _service;

        public GetAllEventLogsQueryHandler(IBaseService<EventLog> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<List<EventLogDisplayDto>>> Handle(GetAllEventLogsQuery request, CancellationToken cancellationToken)
        {
            var items = _service.GetAll();

            if (request.SearchDto != null)
            {
                if (!request.SearchDto.GetAllItems)
                {
                    if (!string.IsNullOrEmpty(request.SearchDto.EntityName))
                        items = items.Where(x => x.EntityName != null && x.EntityName.Contains(request.SearchDto.EntityName));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.EntityId))
                        items = items.Where(x => x.EntityId != null && x.EntityId.Contains(request.SearchDto.EntityId));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.IPAddress))
                        items = items.Where(x => x.IPAddress != null && x.IPAddress.Contains(request.SearchDto.IPAddress));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.Url))
                        items = items.Where(x => x.Url != null && x.Url.Contains(request.SearchDto.Url));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.Method))
                        items = items.Where(x => x.Method != null && x.Method.Contains(request.SearchDto.Method));

                    if (request.SearchDto.EventType.HasValue)
                        items = items.Where(x => x.EventType == request.SearchDto.EventType);

                    if (request.SearchDto.HasError.HasValue)
                        items = items.Where(x => x.HasError == request.SearchDto.HasError);


                    if (!request.SearchDto.Take.HasValue || request.SearchDto.Take <= 0)
                        request.SearchDto.Take = 10;

                    if (!request.SearchDto.Skip.HasValue || request.SearchDto.Skip < 0)
                        request.SearchDto.Skip = 0;

                    items.Take(request.SearchDto.Take.Value).Skip(request.SearchDto.Skip.Value);
                }
            }

            return items.Adapt<List<EventLogDisplayDto>>();
        }
    }
}
