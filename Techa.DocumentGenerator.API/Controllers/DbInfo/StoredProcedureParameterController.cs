using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Commands;
using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.API.Controllers.DbInfo
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class StoredProcedureParameterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<StoredProcedureParameterCreateDto> _createValidator;
        private readonly IValidator<StoredProcedureParameterSearchDto> _searchValidator;

        public StoredProcedureParameterController(IMediator mediator, IValidator<StoredProcedureParameterCreateDto> createValidator, IValidator<StoredProcedureParameterSearchDto> searchValidator)
        {
            this._mediator = mediator;
            this._createValidator = createValidator;
            this._searchValidator = searchValidator;
        }

        [HttpGet]
        public async Task<ApiResult<List<StoredProcedureParameterDisplayDto>>> Get()
        {
            var query = new GetAllStoredProcedureParametersQuery(null);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<StoredProcedureParameterDisplayDto>> Get(int id)
        {
            var query = new GetStoredProcedureParameterQuery(id);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPost("GetByFilter")]
        public async Task<ApiResult<List<StoredProcedureParameterDisplayDto>>> Post(StoredProcedureParameterSearchDto model)
        {
            var result = await _searchValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var query = new GetAllStoredProcedureParametersQuery(model);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPost]
        public async Task<ApiResult<StoredProcedureParameterDisplayDto>> Post(StoredProcedureParameterCreateDto model)
        {
            var result = await _createValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = new CreateStoredProcedureParameterCommand(model);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPut]
        public async Task<ApiResult<StoredProcedureParameterDisplayDto>> Put(StoredProcedureParameterCreateDto model)
        {
            var result = await _createValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = new UpdateStoredProcedureParameterCommand(model);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpDelete]
        public async Task<ApiResult> Delete(int id)
        {
            var command = new DeleteStoredProcedureParameterCommand(id);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
                return Ok();

            return BadRequest(handlerResponse.Message);
        }
    }
}
