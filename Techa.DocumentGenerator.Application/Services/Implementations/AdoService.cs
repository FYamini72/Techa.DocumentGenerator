﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

        public async Task<SQLQueryDisplayDto> GetDataAsync(int? projectId, string query, bool? autoCloseConnection, bool? ignoreLogging, CancellationToken cancellationToken)
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

        public async Task<SQLQueryDisplayDto> ExecuteStoredProcedure(ExecuteStoredProcedureRequestDto model, Dictionary<string, string> outParams, bool? autoCloseConnection, CancellationToken cancellationToken)
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
                return await this.GetDataAsync(model.ProjectId, executableProcedureScript, autoCloseConnection, false, cancellationToken);

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
    }
}
