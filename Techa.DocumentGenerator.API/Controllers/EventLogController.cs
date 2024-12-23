using MediatR;
using Microsoft.AspNetCore.Mvc;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.CQRS.EventLogFiles.Commands;
using Techa.DocumentGenerator.Application.CQRS.EventLogFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos;

namespace Techa.DocumentGenerator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class EventLogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventLogController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResult<List<EventLogDisplayDto>>> Get()
        {
            var query = new GetAllEventLogsQuery(null);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<EventLogDisplayDto>> Get(int id)
        {
            var query = new GetEventLogQuery(id);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPost("GetByFilter")]
        public async Task<ApiResult<List<EventLogDisplayDto>>> Post(EventLogSearchDto model)
        {
            var query = new GetAllEventLogsQuery(model);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPost]
        public async Task<ApiResult<EventLogDisplayDto>> Post(EventLogCreateDto model)
        {
            var command = new CreateEventLogCommand(model);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }
    }
}
