using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
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
                .GetAll(x => x.ProcedureName.ToLower() == model.ProcedureName.ToLower() || (x.Alias != null && x.Alias.ToLower() == model.ProcedureName.ToLower()))
                .FirstOrDefaultAsync();
            if (storedProcedure == null)
                return BadRequest("پروسیجر مورد نظر یافت نشد");

            model.ProcedureName = storedProcedure.ProcedureName;
            
            var storedProcedureParameters = await _storedProcedureParameterService
                .GetAll(x => x.StoredProcedureId == storedProcedure.Id)
                .ToListAsync();

            if (model.Parameters == null)
                model.Parameters = new Dictionary<string, string>();

            model = TranslateAliasNames(storedProcedure, storedProcedureParameters, model);
            
            var userId = _httpContextHelper.GetCurrentUserId();
            string strUserId = userId.HasValue ? userId.Value.ToString() : "";
            if (storedProcedure.ProcedureName.Contains("Insert") || storedProcedure.ProcedureName.Contains("Update"))
            {
                model.Parameters.Add("@CreatedByUserId", strUserId);
                model.Parameters.Add("@ModifiedByUserId", strUserId);
            }

            var result = await _adoService.ExecuteStoredProcedure(model, true, cancellationToken);
            return result;
        }

        [HttpGet("[action]")]
        public async Task<ApiResult<StoredProcedureSummeryResponseDto>> GetStoredProcedureSummery(int storedProcedureId, CancellationToken cancellationToken)
        {
            var ignoredParams = new List<string>() { "@CreatedByUserId".ToLower(), "@ModifiedByUserId".ToLower() };
            var storedProcedure = await _storedProcedureService.GetByIdAsync(cancellationToken, storedProcedureId);
            if (storedProcedure == null)
                return BadRequest("پروسیجر مورد نظر یافت نشد");
            var storedProcedureParameters = await _storedProcedureParameterService
                .GetAll(x => x.StoredProcedureId == storedProcedureId && !ignoredParams.Contains(x.ParameterName.ToLower()))
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
                    Parameters = GenerateParametersDictionary(storedProcedureParameters)
                }
            };

            return Ok(result);
        }

        private ExecuteStoredProcedureRequestDto TranslateAliasNames(StoredProcedure storedProcedure, 
            List<StoredProcedureParameter> storedProcedureParameters, 
            ExecuteStoredProcedureRequestDto model)
        {
            var parameters = new Dictionary<string, string>();

            foreach (var parameter in model.Parameters)
            {
                var param = storedProcedureParameters
                    .FirstOrDefault(x => x.ParameterName.ToLower() == parameter.Key.ToLower() || (x.Alias != null && x.Alias.ToLower() == parameter.Key.ToLower()));
                if (param != null)
                {
                    var parameterKey = param.ParameterName.StartsWith("@") ? param.ParameterName : $"@{param.ParameterName}";
                    var dbParam = storedProcedureParameters.FirstOrDefault(x => x.ParameterName == parameterKey || x.Alias == parameterKey);
                    if (dbParam == null)
                        continue;
                    if (dbParam.ParameterName.ToLower() == "@res")
                        continue;
                    parameters[dbParam.ParameterName] = parameter.Value;
                }
            }

            return new ExecuteStoredProcedureRequestDto()
            {
                ProcedureName = storedProcedure.ProcedureName,
                HasDataTable = model.HasDataTable,
                ProjectId = storedProcedure.ProjectId,
                Parameters = parameters
            };
        }

        private Dictionary<string, string> GenerateParametersDictionary(List<StoredProcedureParameter> parameters)
        {
            var result = new Dictionary<string, string>();

            foreach (var parameter in parameters)
            {
                // اضافه کردن به دیکشنری در صورتی که نام پارامتر معتبر باشد  
                if (!string.IsNullOrWhiteSpace(parameter.ParameterName))
                {
                    if (parameter.ParameterName.ToLower() == "@res")
                        continue;

                    var parameterName = string.IsNullOrEmpty(parameter.Alias) ? parameter.ParameterName : parameter.Alias;
                    result[parameterName] = parameter.DefaultValue ?? "NULL";
                }
            }

            return result;
        }
    }
}
