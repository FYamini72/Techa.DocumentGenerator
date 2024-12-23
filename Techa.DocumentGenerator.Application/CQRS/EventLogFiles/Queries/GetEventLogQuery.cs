using MediatR;
using Techa.DocumentGenerator.Application.Dtos;

namespace Techa.DocumentGenerator.Application.CQRS.EventLogFiles.Queries
{
    public class GetEventLogQuery : IRequest<HandlerResponse<EventLogDisplayDto>>
    {
        public int Id { get; }

        public GetEventLogQuery(int id)
        {
            Id = id;
        }
    }
}