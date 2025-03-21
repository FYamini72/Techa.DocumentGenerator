using Techa.DocumentGenerator.Application.Dtos.DbInfo;

namespace Techa.DocumentGenerator.Application.Repositories
{
    public interface IAdoRepository
    {
        Task<SQLQueryDisplayDto> GetDataAsync(int? projectId, string query, bool? autoCloseConnection, CancellationToken cancellationToken);
        Task<SQLQueryDisplayDto> SetDataAsync(int? projectId, string query, bool? autoCloseConnection, CancellationToken cancellationToken);
    }
}
