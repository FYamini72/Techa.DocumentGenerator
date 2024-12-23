using Techa.DocumentGenerator.Domain.Entities;
using System.Linq.Expressions;

namespace Techa.DocumentGenerator.Application.Services.Interfaces
{
    public interface IBaseService<TEntity>
        where TEntity : class, IBaseEntity
    {
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate = null);

        TEntity GetById(params object[] ids);
        ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

        TEntity Add(TEntity entity, bool saveNow = true);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true);
        void AddRange(IEnumerable<TEntity> entities, bool saveNow = true);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true);

        void Delete(TEntity entity, bool saveNow = true);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true);
        void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true);

        TEntity Update(TEntity entity, bool saveNow = true);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true);
        void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true);
    }
}
