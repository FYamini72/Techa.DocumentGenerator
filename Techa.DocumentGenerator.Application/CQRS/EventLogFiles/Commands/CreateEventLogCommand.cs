using MediatR;
using Techa.DocumentGenerator.Application.Dtos;

namespace Techa.DocumentGenerator.Application.CQRS.EventLogFiles.Commands
{
    public class CreateEventLogCommand : IRequest<HandlerResponse<EventLogDisplayDto>>
    {
        public EventLogCreateDto EventLog { get; }

        public CreateEventLogCommand(EventLogCreateDto Content)
        {
            this.EventLog = Content;
        }
    }
}
