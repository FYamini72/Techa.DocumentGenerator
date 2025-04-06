using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebugger;

namespace Techa.DocumentGenerator.API.Controllers.ExecutionArea;

[Route("api/[controller]")]
[ApiController]
[ApiResultFilter]
public class ScriptDebuggerController(IMediator mediator, IValidator<ScriptDebuggerSearchDto> searchValidator)
    : ControllerBase
{
    [HttpGet]
    public async Task<ApiResult<List<ScriptDebuggerDisplayDto>>> Get()
    {
        var query = new GetAllScriptDebuggersQuery(null);
        var handlerResponse = await mediator.Send(query);

        if (handlerResponse.Status)
            return Ok(handlerResponse.Data);

        return BadRequest(handlerResponse.Message);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResult<ScriptDebuggerDisplayDto>> Get(int id)
    {
        var query = new GetScriptDebuggerQuery(id);
        var handlerResponse = await mediator.Send(query);

        if (handlerResponse.Status)
            return Ok(handlerResponse.Data);

        return BadRequest(handlerResponse.Message);
    }

    [HttpPost("GetByFilter")]
    public async Task<ApiResult<List<ScriptDebuggerDisplayDto>>> Post(ScriptDebuggerSearchDto model)
    {
        var result = await searchValidator.ValidateAsync(model);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var query = new GetAllScriptDebuggersQuery(model);
        var handlerResponse = await mediator.Send(query);

        if (handlerResponse.Status)
            return Ok(handlerResponse.Data);

        return BadRequest(handlerResponse.Message);
    }
}