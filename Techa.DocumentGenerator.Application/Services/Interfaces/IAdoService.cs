using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.Services.Interfaces
{
    public interface IAdoService
    {
        Task<SQLQueryDisplayDto> ExecuteStoredProcedure(ExecuteStoredProcedureRequestDto model, Dictionary<string, string> outParams, bool? autoCloseConnection, CancellationToken cancellationToken);
        Task<SQLQueryDisplayDto> SetDataAsync(int projectId, string query, bool? autoCloseConnection, bool? ignoreLogging, CancellationToken cancellationToken);
        Task<SQLQueryDisplayDto> GetDataAsync(int projectId, string query, bool? autoCloseConnection, bool? ignoreLogging, CancellationToken cancellationToken);
        Task<SQLQueryDisplayDto> GetAllProceduresInfoAsync(int projectId, CancellationToken cancellationToken);
        Task<SQLQueryDisplayDto> GetProcedureInfoAsync(int projectId, string procedureName, CancellationToken cancellationToken);
    }
}
