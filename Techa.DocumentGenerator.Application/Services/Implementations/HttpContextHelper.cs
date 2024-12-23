using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Techa.DocumentGenerator.Application.Services.Interfaces;

namespace Techa.DocumentGenerator.Application.Services.Implementations
{
    public class HttpContextHelper : IHttpContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetCurrentUserId()
        {
            int userId = 0;
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
                if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
                {
                    string authenticatedUserId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0";

                    if (!string.IsNullOrEmpty(authenticatedUserId))
                        int.TryParse(authenticatedUserId, out userId);
                }

            return userId > 0 ? userId : null;
        }

        public string GetIpAddress()
        {
            return _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";
        }

        public string GetRequestMethod()
        {
            return _httpContextAccessor?.HttpContext?.Request?.Method ?? "";
        }

        public string GetRequestPath()
        {
            return _httpContextAccessor?.HttpContext?.Request?.Path ?? "";
        }
    }
}
