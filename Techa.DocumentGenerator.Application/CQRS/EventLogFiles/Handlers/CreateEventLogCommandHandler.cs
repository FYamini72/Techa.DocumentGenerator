using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.CQRS.EventLogFiles.Commands;
using Techa.DocumentGenerator.Application.Dtos;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities;

namespace Techa.DocumentGenerator.Application.CQRS.EventLogFiles.Handlers
{
    public class CreateEventLogCommandHandler : IRequestHandler<CreateEventLogCommand, HandlerResponse<EventLogDisplayDto>>
    {
        private readonly IBaseService<EventLog> _service;

        public CreateEventLogCommandHandler(IBaseService<EventLog> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<EventLogDisplayDto>> Handle(CreateEventLogCommand request, CancellationToken cancellationToken)
        {
            var eventLog = request.EventLog.Adapt<EventLog>();

            var result = await _service.AddAsync(eventLog, cancellationToken);
            return result.Adapt<EventLogDisplayDto>();
        }
    }
}
