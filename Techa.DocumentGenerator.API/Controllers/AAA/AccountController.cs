using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Commands;
using Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.AAA;

namespace Techa.DocumentGenerator.API.Controllers.AAA
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<UserCreateDto> _userCreateValidator;
        private readonly IValidator<UserUpdateDto> _userUpdateValidator;
        private readonly IValidator<UserChangePasswordDto> _userChangePasswordValidator;
        private readonly IValidator<UserSearchDto> _searchValidator;

        public AccountController(IMediator mediator
            , IValidator<LoginDto> loginValidator
            , IValidator<UserCreateDto> userCreateValidator
            , IValidator<UserUpdateDto> userUpdateValidator
            , IValidator<UserChangePasswordDto> userChangePasswordValidator
            , IValidator<UserSearchDto> searchValidator)
        {
            this._mediator = mediator;
            this._loginValidator = loginValidator;
            this._userCreateValidator = userCreateValidator;
            this._userUpdateValidator = userUpdateValidator;
            this._userChangePasswordValidator = userChangePasswordValidator;
            this._searchValidator = searchValidator;
        }

        [HttpGet]
        public async Task<ApiResult<List<UserDisplayDto>>> Get()
        {
            var query = new GetAllUsersQuery(null);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<UserDisplayDto>> Get(int id)
        {
            var query = new GetUserQuery(id);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPost("GetByFilter")]
        public async Task<ApiResult<List<UserDisplayDto>>> Post(UserSearchDto model)
        {
            var result = await _searchValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var query = new GetAllUsersQuery(model);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }


        [HttpPost("[action]")]
        public async Task<ApiResult<UserAndTokenDisplayDto>> Login(LoginDto model)
        {
            var validationResult = await _loginValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var query = new LoginQuery(model.UserName, model.Password, model.ProjectId);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpPost("[action]")]
        public async Task<ApiResult<UserDisplayDto>> CreateUser(UserCreateDto model)
        {
            var validationResult = await _userCreateValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = new CreateUserCommand(model);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }

        [HttpGet("[action]")]
        public async Task<ApiResult<UserDisplayDto>> GetUserByUsername(string Username, int? projectId = null)
        {
            var query = new GetUserByUsernameQuery(Username, projectId);
            var handlerResponse = await _mediator.Send(query);

            if (handlerResponse.Status)
                return Ok(handlerResponse.Data);

            return BadRequest(handlerResponse.Message);
        }


        //[HttpPut]
        //public async Task<ApiResult<UserDisplayDto>> Put(UserUpdateDto model)
        //{
        //    var result = await _userUpdateValidator.ValidateAsync(model);

        //    if (!result.IsValid)
        //    {
        //        result.AddToModelState(ModelState);
        //        return BadRequest(ModelState);
        //    }

        //    var command = new UpdateUserCommand(model);
        //    var handlerResponse = await _mediator.Send(command);

        //    if (handlerResponse.Status)
        //        return Ok(handlerResponse.Data);

        //    return BadRequest(handlerResponse.Message);
        //}

        [HttpPut("[action]")]
        public async Task<ApiResult> ChangePassword(UserChangePasswordDto model)
        {
            var result = await _userChangePasswordValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var command = new ChangePasswordCommand(model);
            var handlerResponse = await _mediator.Send(command);

            if (handlerResponse.Status)
                return Ok();

            return BadRequest(handlerResponse.Message);
        }
    }
}