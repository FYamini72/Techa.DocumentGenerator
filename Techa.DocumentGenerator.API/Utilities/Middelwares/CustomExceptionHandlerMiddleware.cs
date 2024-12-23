using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Utilities.Exceptions;
using System.Net;
using Techa.DocumentGenerator.Domain.Entities;
using Techa.DocumentGenerator.Infrastructure.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Techa.DocumentGenerator.API.Utilities.Middelwares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Microsoft.Extensions.Hosting.IHostingEnvironment _env;
        private ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly bool _isLoggingEnabled;

        public CustomExceptionHandlerMiddleware(RequestDelegate next
            , Microsoft.Extensions.Hosting.IHostingEnvironment env
            , IConfiguration configuration)
        {
            _next = next;
            _env = env;
            _configuration = configuration;

            bool.TryParse(_configuration.GetSection("EventLogConfiguration:IsLoggingEnabled").Value, out _isLoggingEnabled);
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            string message = null;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            ApiResultStatusCode apiStatusCode = ApiResultStatusCode.ServerError;

            try
            {
                await _next(context);
            }
            catch (AppException exception)
            {
                httpStatusCode = exception.HttpStatusCode;
                apiStatusCode = exception.ApiStatusCode;

                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace,
                    };
                    if (exception.InnerException != null)
                    {
                        dic.Add("InnerException.Exception", exception.InnerException.Message);
                        dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
                    }
                    if (exception.AdditionalData != null)
                        dic.Add("AdditionalData", JsonConvert.SerializeObject(exception.AdditionalData));

                    message = JsonConvert.SerializeObject(dic);
                }
                else
                {
                    message = exception.Message;
                }
                await WriteToResponseAsync();
                await AddExceptionLogToEventLogTable();
            }
            catch (SecurityTokenExpiredException exception)
            {
                SetUnAuthorizeResponse(exception);
                await WriteToResponseAsync();
                await AddExceptionLogToEventLogTable();
            }
            catch (UnauthorizedAccessException exception)
            {
                SetUnAuthorizeResponse(exception);
                await WriteToResponseAsync();
                await AddExceptionLogToEventLogTable();
            }
            catch (Exception exception)
            {
                if (_env.IsDevelopment() || _isLoggingEnabled)
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace,
                    };
                    if (exception.InnerException != null)
                    {
                        dic.Add("InnerException.Exception", exception.InnerException.Message);
                        dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
                    }
                    message = JsonConvert.SerializeObject(dic);

                    await AddExceptionLogToEventLogTable();
                }

                if(!_env.IsDevelopment())
                    message = null;

                await WriteToResponseAsync();
            }

            async Task AddExceptionLogToEventLogTable()
            {
                try
                {
                    if (_isLoggingEnabled)
                    {
                        string? ipAddress = null;
                        if (context.Connection.RemoteIpAddress != null)
                            ipAddress = context.Connection.RemoteIpAddress.ToString();

                        string? requestBody = null;

                        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                        {
                            var postData = await reader.ReadToEndAsync();
                            if (!string.IsNullOrEmpty(postData))
                                requestBody = postData.Trim();
                        }

                        var log = new EventLog()
                        {
                            DataJson = requestBody,
                            Message = message,
                            Url = context.Request.Path,
                            Method = context.Request.Method,
                            HasError = true,
                            IPAddress = ipAddress,
                            EventType = Domain.Enums.EventType.Exception
                        };

                        var changedEntriesCopy = _dbContext.ChangeTracker.Entries().Where(e => e.State != EntityState.Detached).ToList();

                        foreach (var entry in changedEntriesCopy)
                            entry.State = EntityState.Detached;

                        await _dbContext.Set<EventLog>().AddAsync(log);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                catch { }
            }
            async Task WriteToResponseAsync()
            {
                if (context.Response.HasStarted)
                    throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

                var result = new ApiResult(false, apiStatusCode, message);
                var json = JsonConvert.SerializeObject(result);

                context.Response.StatusCode = (int)httpStatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }

            void SetUnAuthorizeResponse(Exception exception)
            {
                httpStatusCode = HttpStatusCode.Unauthorized;
                apiStatusCode = ApiResultStatusCode.UnAuthorized;

                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace
                    };
                    if (exception.InnerException != null)
                    {
                        dic.Add("InnerException.Exception", exception.InnerException.Message);
                        dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
                    }
                    if (exception is SecurityTokenExpiredException tokenException)
                        dic.Add("Expires", tokenException.Expires.ToString());

                    message = JsonConvert.SerializeObject(dic);
                }
            }
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
