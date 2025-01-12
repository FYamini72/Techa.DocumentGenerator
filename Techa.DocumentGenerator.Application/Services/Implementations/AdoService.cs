using Microsoft.Extensions.Configuration;
using System.Text;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Application.Repositories;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities;

namespace Techa.DocumentGenerator.Application.Services.Implementations
{
    public class AdoService : IAdoService
    {
        private readonly IAdoRepository _adoRepository;
        private readonly IBaseService<EventLog> _eventLogService;
        private readonly IConfiguration _configuration;

        private readonly string? _ipAddress;
        private readonly string? _url;
        private readonly string? _method;

        private readonly bool _isLoggingEnabled;

        public AdoService(IAdoRepository adoRepository, IConfiguration configuration, IBaseService<EventLog> eventLogService, IHttpContextHelper httpContextHelper)
        {
            _adoRepository = adoRepository;
            _configuration = configuration;
            _eventLogService = eventLogService;

            bool.TryParse(_configuration.GetSection("EventLogConfiguration:IsLoggingEnabled").Value, out _isLoggingEnabled);
            _ipAddress = httpContextHelper.GetIpAddress();
            _url = httpContextHelper.GetRequestPath();
            _method = httpContextHelper.GetRequestMethod();
        }

        public async Task<SQLQueryDisplayDto> GetDataAsync(int projectId, string query, bool? autoCloseConnection, bool? ignoreLogging, CancellationToken cancellationToken)
        {
            autoCloseConnection = autoCloseConnection ?? true;
            ignoreLogging = ignoreLogging ?? false;

            var result = await _adoRepository.GetDataAsync(projectId, query, autoCloseConnection, cancellationToken);

            if (_isLoggingEnabled && !ignoreLogging.Value)
            {
                var log = new EventLog()
                {
                    HasError = result.HasError,
                    Message = result.Messages,
                    EventType = Domain.Enums.EventType.TSqlExecution,
                    ExecutedScript = result.Script,
                    IPAddress = _ipAddress,
                    Url = _url,
                    Method = _method,
                    DataJson = result.Dataset
                };

                await _eventLogService.AddAsync(log, cancellationToken);
            }

            return result;
        }

        public async Task<SQLQueryDisplayDto> SetDataAsync(int projectId, string query, bool? autoCloseConnection, bool? ignoreLogging, CancellationToken cancellationToken)
        {
            autoCloseConnection = autoCloseConnection ?? true;
            ignoreLogging = ignoreLogging ?? false;

            var result = await _adoRepository.SetDataAsync(projectId, query, autoCloseConnection, cancellationToken);

            if (_isLoggingEnabled && !ignoreLogging.Value)
            {
                var log = new EventLog()
                {
                    HasError = result.HasError,
                    Message = result.Messages,
                    EventType = Domain.Enums.EventType.TSqlExecution,
                    ExecutedScript = result.Script,
                    IPAddress = _ipAddress,
                    Url = _url,
                    Method = _method,
                    DataJson = result.Dataset
                };

                await _eventLogService.AddAsync(log, cancellationToken);
            }

            return result;
        }

        public async Task<SQLQueryDisplayDto> ExecuteStoredProcedure(ExecuteStoredProcedureRequestDto model, bool? autoCloseConnection, CancellationToken cancellationToken)
        {
            autoCloseConnection = autoCloseConnection ?? true;

            var parametersWithValue = new List<string>();

            if (model.Parameters != null && model.Parameters.Any())
                foreach (var parameter in model.Parameters)
                {
                    string paramValue = '\'' + parameter.Value + '\'';
                    if (double.TryParse(parameter.Value, out double numericValue))
                        paramValue = numericValue.ToString();

                    parametersWithValue.Add($"{parameter.Key} = {paramValue}");
                }

            string executableProcedureScript = string.Empty;

            if (model.ProcedureName.Contains("Insert") || model.ProcedureName.Contains("Update"))
                executableProcedureScript += "DECLARE\t@res NVARCHAR(MAX)\r\n";

            executableProcedureScript += $"EXEC {model.ProcedureName} {string.Join(", ", parametersWithValue)}";

            if (model.ProcedureName.Contains("Insert") || model.ProcedureName.Contains("Update"))
                executableProcedureScript += ", @res = @res OUTPUT\r\nSELECT @res as N'@res'";

            if (model.HasDataTable ?? true)
                return await this.GetDataAsync(model.ProjectId, executableProcedureScript, autoCloseConnection, false, cancellationToken);

            return await this.SetDataAsync(model.ProjectId, executableProcedureScript, autoCloseConnection, false, cancellationToken);
        }
    }
}
