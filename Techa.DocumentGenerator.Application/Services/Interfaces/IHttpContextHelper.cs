namespace Techa.DocumentGenerator.Application.Services.Interfaces
{
    public interface IHttpContextHelper
    {
        int? GetCurrentUserId();
        string GetIpAddress();
        string GetRequestPath();
        string GetRequestMethod();
    }
}
