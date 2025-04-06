using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Application.Repositories;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;

namespace Techa.DocumentGenerator.Application.Services.Implementations
{
    public class AdoService : IAdoService
    {
        private readonly IAdoRepository _adoRepository;
        private readonly IBaseService<EventLog> _eventLogService;
        private readonly IConfiguration _configuration;
        private readonly IBaseService<ScriptDebugger> _scriptDebuggerService;

        private readonly string? _ipAddress;
        private readonly string? _url;
        private readonly string? _method;

        private readonly bool _isLoggingEnabled;
        private readonly bool _isDebuggerEnabled;
        private readonly int? _userId;

        public AdoService(IAdoRepository adoRepository,
            IConfiguration configuration,
            IBaseService<EventLog> eventLogService,
            IHttpContextHelper httpContextHelper,
            IBaseService<ScriptDebugger> scriptDebuggerService)
        {
            _adoRepository = adoRepository;
            _configuration = configuration;
            _eventLogService = eventLogService;
            _scriptDebuggerService = scriptDebuggerService;

            bool.TryParse(_configuration.GetSection("EventLogConfiguration:IsLoggingEnabled").Value, out _isLoggingEnabled);
            bool.TryParse(configuration.GetSection("DebuggerConfiguration:IsSqlDebuggerEnabled").Value, out _isDebuggerEnabled);

            _ipAddress = httpContextHelper.GetIpAddress();
            _url = httpContextHelper.GetRequestPath();
            _method = httpContextHelper.GetRequestMethod();
            _userId = httpContextHelper.GetCurrentUserId();
        }

        public async Task<SQLQueryDisplayDto> GetDataAsync(int? projectId,
            string query,
            bool? autoCloseConnection,
            bool? ignoreLogging,
            bool? ignoreDebugger,
            CancellationToken cancellationToken)
        {
            autoCloseConnection = autoCloseConnection ?? true;
            ignoreLogging = ignoreLogging ?? false;
            ignoreDebugger = ignoreDebugger ?? false;

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

            if (_isDebuggerEnabled && !ignoreDebugger.Value)
            {
                var debugger = new ScriptDebugger
                {
                    Script = result.Script,
                    UserId = _userId,
                    ProjectId = projectId,
                    ScriptDebuggerDetails = GetScriptDebuggerDetails(result)
                };

                _scriptDebuggerService.Add(debugger);
            }

            return result;
        }

        private List<ScriptDebuggerDetail> GetScriptDebuggerDetails(SQLQueryDisplayDto result)
        {
            var messageLines = result.Messages.Split("\r\n").Where(x => x.Trim() != "" && x.StartsWith("pv:")).Select(x => x.Trim()).ToList();

            return messageLines.Select(line => new ScriptDebuggerDetail { ResultValue = line.Split("pv:")[1].Trim() }).ToList();
        }

        public async Task<SQLQueryDisplayDto> SetDataAsync(int? projectId, string query, bool? autoCloseConnection, bool? ignoreLogging, CancellationToken cancellationToken)
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

        public async Task<SQLQueryDisplayDto> DropTempProcedure(DropStoredProcedureRequestDto model, CancellationToken cancellationToken)
        {
            return await _adoRepository.SetDataAsync(model.ProjectId, $"DROP PROC Tmp_{model.ProcedureName}", true, cancellationToken);
        }

        public async Task<SQLQueryDisplayDto> ExecuteStoredProcedureWithDebugging(ExecuteStoredProcedureRequestDto model, Dictionary<string, string> outParams, bool? autoCloseConnection, CancellationToken cancellationToken)
        {
            var newProcedureBody = await GenerateTempProcedureBody(model, cancellationToken);
            model.ProcedureName = "Tmp_" + model.ProcedureName;

            var createTempProcedureResult = await _adoRepository.SetDataAsync(model.ProjectId, newProcedureBody, autoCloseConnection, cancellationToken);
            if (createTempProcedureResult.HasError)
                return createTempProcedureResult;

            return await ExecuteStoredProcedure(model, outParams, autoCloseConnection, false, false, cancellationToken);
        }

        public async Task<SQLQueryDisplayDto> ExecuteStoredProcedure(ExecuteStoredProcedureRequestDto model,
            Dictionary<string, string> outParams,
            bool? autoCloseConnection,
            bool? ignoreLogging,
            bool? ignoreDebugger,
            CancellationToken cancellationToken)
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

            foreach (var outParam in outParams)
            {
                // Key: ParameterName - Value: ParameterType
                executableProcedureScript += $"DECLARE\t{outParam.Key} {outParam.Value}\r\n";
            }

            executableProcedureScript += $"EXEC {model.ProcedureName} {string.Join(", ", parametersWithValue)}";

            foreach (var outParam in outParams)
            {
                // Key: ParameterName - Value: ParameterType
                executableProcedureScript += $", {outParam.Key} = {outParam.Key} OUTPUT\r\nSELECT {outParam.Key} AS N'{outParam.Key}'";
            }

            if (model.HasDataTable ?? true)
                return await this.GetDataAsync(model.ProjectId, executableProcedureScript, autoCloseConnection, ignoreLogging, ignoreDebugger, cancellationToken);

            return await this.SetDataAsync(model.ProjectId, executableProcedureScript, autoCloseConnection, false, cancellationToken);
        }

        public async Task<SQLQueryDisplayDto> GetAllProceduresInfoAsync(int? projectId, CancellationToken cancellationToken)
        {
            string sqlQuery = @"SELECT p.name AS ProcedureName, NULL AS ProcedureCode FROM sys.procedures AS p";

            return await _adoRepository.GetDataAsync(projectId, sqlQuery, true, cancellationToken);
        }

        public async Task<SQLQueryDisplayDto> GetProcedureInfoAsync(int? projectId, string procedureName, CancellationToken cancellationToken)
        {
            string sqlQuery = @"  
                SELECT   
                    p.name AS ProcedureName,  
                    m.definition AS ProcedureCode  
                FROM   
                    sys.procedures AS p  
                INNER JOIN   
                    sys.sql_modules AS m ON p.object_id = m.object_id
                WHERE p.name = N'{0}'";

            return await _adoRepository.GetDataAsync(projectId, (string.Format(sqlQuery, procedureName)), true, cancellationToken);
        }

        private async Task<string> GenerateTempProcedureBody(ExecuteStoredProcedureRequestDto model, CancellationToken cancellationToken)
        {
            var procedureInfoResult = await GetProcedureInfoAsync(model.ProjectId, model.ProcedureName, cancellationToken);
            if (procedureInfoResult is null || procedureInfoResult.HasError)
                return string.Empty;
            var procedures = JsonConvert.DeserializeObject<List<StoredProcedureScriptDto>>(procedureInfoResult.Dataset);
            if (procedures == null || !procedures.Any())
                return string.Empty;
            var procedure = procedures.First();
            if (string.IsNullOrEmpty(procedure.ProcedureCode))
                return string.Empty;

            string procedureBody = Regex.Replace(
                procedure.ProcedureCode,
                "ALTER PROC",
                "CREATE PROC",
                RegexOptions.IgnoreCase
            );

            procedureBody = Regex.Replace(
                procedure.ProcedureCode,
                model.ProcedureName,
                "Tmp_" + model.ProcedureName,
                RegexOptions.IgnoreCase
            );

            var queryBuilder = new StringBuilder();
            queryBuilder.Append(procedureBody);

            int indexer = 0;

            foreach (var lineToDebug in model.LinesToDebug.OrderBy(x => (int)x.ScriptType))
            {
                if (lineToDebug.ScriptType != Domain.Enums.ScriptType.Select)
                {
                    continue;
                }

                var script = lineToDebug.Script;

                var jsonVariableName = $"@JsonResult_{indexer}";
                var newValue = $"DECLARE {jsonVariableName} NVARCHAR(MAX);  \r\n\r\nSELECT {jsonVariableName} = ({script.Replace(";", " ")} FOR JSON PATH );  \r\n\r\nPRINT 'pv: ' + {jsonVariableName};";

                queryBuilder = queryBuilder.Replace(script, script + "\r\n" + newValue);
            }

            return queryBuilder.ToString();
        }
    }
}
