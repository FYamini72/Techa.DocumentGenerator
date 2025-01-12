using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Filters;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;

namespace Techa.DocumentGenerator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    [Authorize]
    public class ExecuteTSqlController : ControllerBase
    {
        private readonly IAdoService _adoService;
        private readonly IHttpContextHelper _httpContextHelper;

        public ExecuteTSqlController(IAdoService adoService, IHttpContextHelper httpContextHelper)
        {
            _adoService = adoService;
            _httpContextHelper = httpContextHelper;
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
            if (model.Parameters == null)
                model.Parameters = new Dictionary<string, string>();
            var userId = _httpContextHelper.GetCurrentUserId();
            string strUserId = userId.HasValue ? userId.Value.ToString() : "";
            if (model.ProcedureName.Contains("Insert") || model.ProcedureName.Contains("Update"))
            {
                model.Parameters.Add("@CreatedByUserId", strUserId);
                model.Parameters.Add("@ModifiedByUserId", strUserId);
            }
            var result = await _adoService.ExecuteStoredProcedure(model, true, cancellationToken);
            return result;
        }

    }
}
