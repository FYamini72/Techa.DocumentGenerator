using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.Dtos;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities;

namespace Techa.DocumentGenerator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class BaseController<TEntity, TDisplayDto, TCreateDto, TSearchDto> : ControllerBase
        where TEntity : BaseEntity, new()
        where TDisplayDto : BaseDto, new()
        where TCreateDto : BaseDto, new()
        where TSearchDto : BaseSearchDto, new()
    {
        private readonly IBaseService<TEntity> _service;
        private readonly IValidator<TCreateDto> _createValidator;
        private readonly IValidator<TSearchDto> _searchValidator;

        public BaseController(IBaseService<TEntity> service, IValidator<TCreateDto> createValidator, IValidator<TSearchDto> searchValidator)
        {
            this._service = service;
            this._createValidator = createValidator;
            this._searchValidator = searchValidator;
        }


        [HttpGet]
        public virtual async Task<ApiResult<List<TDisplayDto>>> Get()
        {
            var result = _service.GetAll().AsNoTracking();
            return Ok(result.Adapt<List<TDisplayDto>>());
        }

        [HttpGet("{id:int}")]
        public virtual async Task<ApiResult<TDisplayDto>> Get(int id, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, id);

            if (obj != null)
                return Ok(obj.Adapt<TDisplayDto>());

            return BadRequest("رکورد موردنظر یافت نشد");
        }

        [HttpPost("GetByFilter")]
        public virtual async Task<ApiResult<List<TDisplayDto>>> Post(TSearchDto model, CancellationToken cancellationToken)
        {
            var validationResult = await _searchValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var result = _service.GetAll();

            if (!model.GetAllItems)
            {
                result = FilterResult(model, result, cancellationToken);

                if (!model.Take.HasValue || model.Take <= 0)
                    model.Take = 10;

                if (!model.Skip.HasValue || model.Skip < 0)
                    model.Skip = 0;

                result.Take(model.Take.Value).Skip(model.Skip.Value);
            }

            return Ok(result.Adapt<List<TDisplayDto>>());
        }

        [NonAction]
        public virtual IQueryable<TEntity> FilterResult(TSearchDto model, IQueryable<TEntity> result, CancellationToken cancellationToken)
        {
            return result;
        }

        [HttpPost]
        public virtual async Task<ApiResult<TDisplayDto>> Post(TCreateDto model, CancellationToken cancellationToken)
        {
            var validationResult = await _createValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var obj = model.Adapt<TEntity>();
            var result = await _service.AddAsync(obj, cancellationToken);
            
            return Ok(result.Adapt<TDisplayDto>());
        }

        [HttpPut]
        public virtual async Task<ApiResult<TDisplayDto>> Put(TCreateDto model, CancellationToken cancellationToken)
        {
            var validationResult = await _createValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var obj = await _service.GetByIdAsync(cancellationToken, model.Id);

            if (obj == null)
                return BadRequest("رکورد مورد نظر یافت نشد");

            model.Adapt(obj);
            var result = await _service.UpdateAsync(obj, cancellationToken);

            return Ok(result.Adapt<TDisplayDto>());
        }

        [HttpDelete]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, id);

            if (obj == null)
                return BadRequest("رکورد موردنظر یافت نشد");

            await _service.DeleteAsync(obj, cancellationToken);

            return Ok();
        }
    }
}
