using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebuggerDetail;

namespace Techa.DocumentGenerator.API.Controllers.ExecutionArea;

[Route("api/[controller]")]
[ApiController]
[ApiResultFilter]
public class ScriptDebuggerDetailController(
    IMediator mediator,
    IValidator<ScriptDebuggerDetailSearchDto> searchValidator)
    : ControllerBase
{
    [HttpGet]
    public async Task<ApiResult<List<ScriptDebuggerDetailDisplayDto>>> Get()
    {
        var query = new GetAllScriptDebuggerDetailsQuery(null);
        var handlerResponse = await mediator.Send(query);

        if (handlerResponse.Status)
            return Ok(handlerResponse.Data);

        return BadRequest(handlerResponse.Message);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResult<ScriptDebuggerDetailDisplayDto>> Get(int id)
    {
        var query = new GetScriptDebuggerDetailQuery(id);
        var handlerResponse = await mediator.Send(query);

        if (handlerResponse.Status)
            return Ok(handlerResponse.Data);

        return BadRequest(handlerResponse.Message);
    }

    [HttpPost("GetByFilter")]
    public async Task<ApiResult<List<ScriptDebuggerDetailDisplayDto>>> Post(ScriptDebuggerDetailSearchDto model)
    {
        var result = await searchValidator.ValidateAsync(model);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var query = new GetAllScriptDebuggerDetailsQuery(model);
        var handlerResponse = await mediator.Send(query);

        if (handlerResponse.Status)
            return Ok(handlerResponse.Data);

        return BadRequest(handlerResponse.Message);
    }
}