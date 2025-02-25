using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Commands;
using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.API.Controllers.DbInfo
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class StoredProcedureController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<StoredProcedureCreateDto> _createValidator;
        private readonly IValidator<StoredProcedureSearchDto> _searchValidator;
        private readonly IValidator<StoredProcedureScriptDto> _scriptValidator;

        public StoredProcedureController(IMediator mediator, 
            IValidator<StoredProcedureCreateDto> createValidator, 
            IValidator<StoredProcedureSearchDto> searchValidator, 
            IValidator<StoredProcedureScriptDto> scriptValidator)
        {
            this._mediator = mediator;
            this._createValidator = createValidator;
            this._searchValidator = searchValidator;
            this._scriptValidator = scriptValidator;
        }

        [HttpGet]
        public async Task<ApiResult<BaseGridDto<StoredProcedureDisplayDto>>> Get()
        {
            var query = new GetAllStoredProceduresQuery(null);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<StoredProcedureDisplayDto>> Get(int id)
        {
            var query = new GetStoredProcedureQuery(id);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPost("GetByFilter")]
        public async Task<ApiResult<BaseGridDto<StoredProcedureDisplayDto>>> Post(StoredProcedureSearchDto model)
        {
            var result = await _searchValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var query = new GetAllStoredProceduresQuery(model);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPost]
        public async Task<ApiResult<StoredProcedureDisplayDto>> Post(StoredProcedureCreateDto model)
        {
            var result = await _createValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = new CreateStoredProcedureCommand(model);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPut]
        public async Task<ApiResult<StoredProcedureDisplayDto>> Put(StoredProcedureCreateDto model)
        {
            var result = await _createValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = new UpdateStoredProcedureCommand(model);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpDelete]
        public async Task<ApiResult> Delete(int id)
        {
            var command = new DeleteStoredProcedureCommand(id);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
                return Ok();

            return BadRequest(handlerResponse.Message);
        }

        [HttpGet("[action]")]
        public async Task<ApiResult> GenerateStoredProcedureParametersUsingAi(int id, CancellationToken cancellationToken)
        {
            var query = new GenerateStoredProcedureParametersUsingAiQuery(id);
            var handlerResponse = await _mediator.Send(query, cancellationToken);

            if (handlerResponse.Status)
                return Ok();

            return BadRequest(handlerResponse.Message);
        }

        [HttpGet("[action]")]
        public async Task<ApiResult<StoredProcedureScriptDto>> GetStoredProcedureScript(int id, CancellationToken cancellationToken)
        {
            var query = new GetStoredProcedureScriptQuery(id);
            var handlerResponse = await _mediator.Send(query, cancellationToken);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPut("[action]")]
        public async Task<ApiResult> EditStoredProcedureScript(StoredProcedureScriptDto model, CancellationToken cancellationToken)
        {
            var result = await _scriptValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = new EditStoredProcedureScriptCommand(model);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
            {
                var query = new GenerateStoredProcedureParametersUsingAiQuery(model.Id);
                var updateParametersHandlerResponse = await _mediator.Send(query, cancellationToken);

                if (!updateParametersHandlerResponse.Status)
                    return BadRequest(updateParametersHandlerResponse.Message);

                return Ok();
            }

            return BadRequest(handlerResponse.Message);
        }
    }
}
