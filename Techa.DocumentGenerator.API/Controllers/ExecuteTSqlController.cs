using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Application.Services.Implementations;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Domain.Enums;

namespace Techa.DocumentGenerator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    //[Authorize]
    public class ExecuteTSqlController : ControllerBase
    {
        private readonly IAdoService _adoService;
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IBaseService<StoredProcedure> _storedProcedureService;
        private readonly IBaseService<StoredProcedureParameter> _storedProcedureParameterService;
        private readonly IConfiguration _configuration;

        public ExecuteTSqlController(IAdoService adoService,
            IHttpContextHelper httpContextHelper,
            IBaseService<StoredProcedure> storedProcedureService,
            IBaseService<StoredProcedureParameter> storedProcedureParameterService,
            IConfiguration configuration)
        {
            _adoService = adoService;
            _httpContextHelper = httpContextHelper;
            _storedProcedureService = storedProcedureService;
            _storedProcedureParameterService = storedProcedureParameterService;
            _configuration = configuration;
        }

        /// <summary>
        /// اطلاعات مورد نیاز برای اجرای یک رویه ذخیره شده را دریافت و بعد از اعمال اعتبارسنجی های لازم آن را اجرا می‌کند
        /// </summary>
        /// <param name="model">اطلاعات مورد نیاز برای اجرای رویه ذخیره شده شامل نام و پارامترهای ورودی</param>
        /// <param name="cancellationToken">توکن لغو فرآیند، درصورتی که به هر دلیلی کلاینت تصمیم به لغو درخواست بگیرد، با استفاده از این توکن فرآیند لغو می‌گردد</param>
        /// <returns>
        /// درصورت اجرای بدون مشکل درخواست، خروجی تولید شده از طرف رویه ذخیره شده برگشت داده می‌شود
        /// </returns>
        [HttpPost("[action]")]
        public async Task<ApiResult<SQLQueryDisplayDto>> ExecuteStoredProcedure(ExecuteStoredProcedureRequestDto model, CancellationToken cancellationToken)
        {
            var storedProcedure = await _storedProcedureService
                .GetAll(x => 
                    x.ProjectId == model.ProjectId && 
                    (x.ProcedureName.ToLower() == model.ProcedureName.ToLower() || (x.Alias != null && x.Alias.ToLower() == model.ProcedureName.ToLower()))
                )
                .FirstOrDefaultAsync();

            if (storedProcedure == null)
                return BadRequest("پروسیجر مورد نظر یافت نشد");

            model.ProcedureName = storedProcedure.ProcedureName;

            var parametersQuery = _storedProcedureParameterService
                .GetAll(x => x.StoredProcedureId == storedProcedure.Id);
                
            var storedProcedureParameters = await parametersQuery.ToListAsync();
            var outParameters = await parametersQuery
                .Where(x => x.IsOutParameter)
                .Select(x => new KeyValuePair<string, string>(
                    x.ParameterName.StartsWith("@") ? x.ParameterName : "@" + x.ParameterName,
                    x.ParameterType ?? "NVARCHAR(MAX)")
                )
                .ToListAsync();

            if (model.Parameters == null)
                model.Parameters = new Dictionary<string, string>();

            model.Parameters = TranslateAliasNames(model.Parameters, storedProcedureParameters);
            
            var userId = _httpContextHelper.GetCurrentUserId();
            string strUserId = userId.HasValue ? userId.Value.ToString() : "";
            
            if (
                storedProcedure.ProcedureName.ToLower().StartsWith("save_") && 
                (
                    storedProcedureParameters.Any(x=>x.ParameterName.ToLower() == "CreatedByUserId".ToLower()) || 
                    storedProcedureParameters.Any(x => x.ParameterName.ToLower() == "ModifiedByUserId".ToLower())
                )
            )
            {
                model.Parameters.Add("@CreatedByUserId", strUserId);
                model.Parameters.Add("@ModifiedByUserId", strUserId);
            }

            if (model.LinesToDebug != null && model.LinesToDebug.Any())
                return await _adoService.ExecuteStoredProcedureWithDebugging(model, outParameters.ToDictionary(), true, cancellationToken);
            return await _adoService.ExecuteStoredProcedure(model, outParameters.ToDictionary(), true, false, true, cancellationToken);
        }

        [HttpGet("[action]")]
        public async Task<ApiResult<StoredProcedureSummeryResponseDto>> GetStoredProcedureSummery(int storedProcedureId, CancellationToken cancellationToken)
        {
            var ignoredParams = new List<string>() { "@CreatedByUserId".ToLower(), "@ModifiedByUserId".ToLower() };
            var storedProcedure = await _storedProcedureService.GetByIdAsync(cancellationToken, storedProcedureId);
            if (storedProcedure == null)
                return BadRequest("پروسیجر مورد نظر یافت نشد");
            var storedProcedureParameters = await _storedProcedureParameterService
                .GetAll(x => 
                    x.StoredProcedureId == storedProcedureId && 
                    !ignoredParams.Contains(x.ParameterName.ToLower()) && 
                    !x.IsOutParameter
                )
                .ToListAsync();

            bool hasDataTable = storedProcedure.StoredProcedureType == StoredProcedureType.NotSet || storedProcedure.StoredProcedureType == StoredProcedureType.Read;
            var result = new StoredProcedureSummeryResponseDto()
            {
                Url = _configuration.GetSection("StoredProcedureExecutionUrl")?.Value ?? "",
                HttpMethod = "POST",
                InputData = new()
                {
                    ProjectId = storedProcedure.ProjectId,
                    ProcedureName = storedProcedure.Alias ?? storedProcedure.ProcedureName,
                    HasDataTable = hasDataTable,
                    Parameters = GenerateParametersDictionary(storedProcedureParameters),
                    LinesToDebug = new() { new() }
                }
            };

            return Ok(result);
        }
        
        [HttpPost("[action]")]
        public async Task<ApiResult<SQLQueryDisplayDto>> DropTempProcedure(DropStoredProcedureRequestDto model, CancellationToken cancellationToken)
        {
            return await _adoService.DropTempProcedure(model, cancellationToken);
        }

        //[HttpPost("[action]")]
        //public async Task<ApiResult<SQLQueryDisplayDto>> ExecuteStoredProcedureWithDebugger(StoredProcedureWithDebuggerDto model)
        //{
        //    return _adoService.ExecuteStoredProcedure(model);
        //    result.AddToModelState(ModelState);
        //    return BadRequest(ModelState);

        //}

        private Dictionary<string, string> TranslateAliasNames(Dictionary<string, string> inputParameters, List<StoredProcedureParameter> storedProcedureParameters)
        {
            var parameters = new Dictionary<string, string>();

            foreach (var parameter in inputParameters)
            {
                var param = storedProcedureParameters
                    .FirstOrDefault(x => x.ParameterName.ToLower() == parameter.Key.ToLower() || (x.Alias != null && x.Alias.ToLower() == parameter.Key.ToLower()));
                if (param != null)
                {
                    var parameterKey = param.ParameterName.StartsWith("@") ? param.ParameterName : $"@{param.ParameterName}";
                    //var dbParam = storedProcedureParameters.FirstOrDefault(x => x.ParameterName == parameterKey || x.Alias == parameterKey);
                    //if (dbParam == null)
                    //    continue;
                    //if (param.ParameterName.ToLower() == "@res")
                    //    continue;
                    parameters[param.ParameterName] = parameter.Value;
                }
            }

            return parameters;
        }

        private Dictionary<string, string> GenerateParametersDictionary(List<StoredProcedureParameter> parameters)
        {
            var result = new Dictionary<string, string>();

            foreach (var parameter in parameters)
            {
                // اضافه کردن به دیکشنری در صورتی که نام پارامتر معتبر باشد  
                if (!string.IsNullOrWhiteSpace(parameter.ParameterName))
                {
                    var parameterName = string.IsNullOrEmpty(parameter.Alias) ? parameter.ParameterName : parameter.Alias;
                    string defaultValue = string.IsNullOrEmpty(parameter.DefaultValue)
                        ? "null"
                        : (parameter.DefaultValue.ToLower() == "null" ? "null" : parameter.DefaultValue);
                    result[parameterName] = defaultValue;
                }
            }

            return result;
        }
    }
}
