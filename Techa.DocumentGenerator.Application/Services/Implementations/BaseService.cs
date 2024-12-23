using Techa.DocumentGenerator.Application.Repositories;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;


namespace Techa.DocumentGenerator.Application.Services.Implementations
{
    public class BaseService<TEntity> : IBaseService<TEntity>
        where TEntity : class, IBaseEntity
    {
        private readonly IBaseRepository<TEntity> _repository;
        private readonly IBaseRepository<EventLog> _eventLogRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly bool _logCreateEvent;
        private readonly bool _logUpdateEvent;
        private readonly bool _logDeleteEvent;

        private readonly string? _ipAddress;
        private readonly string? _url;
        private readonly string? _method;

        public BaseService(IBaseRepository<TEntity> repository
            , IBaseRepository<EventLog> eventLogRepository
            , IConfiguration configuration
            , IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _eventLogRepository = eventLogRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            bool isLoggingEnabled = false;
            bool.TryParse(_configuration.GetSection("EventLogConfiguration:IsLoggingEnabled").Value, out isLoggingEnabled);

            //_logCreateEvent = isLoggingEnabled ? typeof(TEntity).GetCustomAttributes : false;
            _logCreateEvent = isLoggingEnabled ? typeof(TEntity).GetInterfaces().Contains(typeof(ILogCreateEvent)) : false;
            _logUpdateEvent = isLoggingEnabled ? typeof(TEntity).GetInterfaces().Contains(typeof(ILogUpdateEvent)) : false;
            _logDeleteEvent = isLoggingEnabled ? typeof(TEntity).GetInterfaces().Contains(typeof(ILogDeleteEvent)) : false;


            _ipAddress = "";
            _url = "";
            _method = "";

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                _url = httpContext.Request.Path;
                _method = httpContext.Request.Method;

                if (httpContext.Connection != null && httpContext.Connection.RemoteIpAddress != null)
                    _ipAddress = httpContext.Connection.RemoteIpAddress.ToString();
            }
        }

        public virtual TEntity Add(TEntity entity, bool saveNow = true)
        {
            var result = _repository.Add(entity, saveNow);

            if (_logCreateEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(result);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        DataJson = strEntity,
                        EntityId = result.Id.ToString(),
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.Create
                    };

                    _eventLogRepository.Add(obj);
                }
                catch (Exception) { throw; }
            }

            return result;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            var result = (await _repository.AddAsync(entity, cancellationToken, saveNow));

            if (_logCreateEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(result);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        DataJson = strEntity,
                        EntityId = result.Id.ToString(),
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.Create
                    };

                    await _eventLogRepository.AddAsync(obj, cancellationToken);
                }
                catch (Exception) { throw; }
            }

            return result;
        }

        public virtual void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            _repository.AddRange(entities, saveNow);

            if (_logCreateEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entities);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = string.Join(", ", entities.Where(x => x.Id > 0).Select(x => x.Id)),
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.CreateRange
                    };

                    _eventLogRepository.Add(obj);
                }
                catch (Exception) { throw; }
            }
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            await _repository.AddRangeAsync(entities, cancellationToken, saveNow);

            if (_logCreateEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entities);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = string.Join(", ", entities.Where(x => x.Id > 0).Select(x => x.Id)),
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.CreateRange
                    };

                    await _eventLogRepository.AddAsync(obj, cancellationToken);
                }
                catch (Exception) { throw; }
            }
        }

        public virtual void Delete(TEntity entity, bool saveNow = true)
        {
            int deletedId = entity.Id;

            _repository.Delete(entity, saveNow);

            if (_logDeleteEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entity);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = deletedId.ToString(),
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.Delete
                    };

                    _eventLogRepository.Add(obj);
                }
                catch (Exception) { throw; }
            }
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            int deletedId = entity.Id;

            await _repository.DeleteAsync(entity, cancellationToken, saveNow);

            if (_logDeleteEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entity);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = deletedId.ToString(),
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.Delete
                    };

                    await _eventLogRepository.AddAsync(obj, cancellationToken);
                }
                catch (Exception) { throw; }
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            string deletedIds = string.Join(", ", entities.Where(x => x.Id > 0).Select(x => x.Id));

            _repository.DeleteRange(entities, saveNow);

            if (_logDeleteEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entities);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = deletedIds,
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.DeleteRange
                    };

                    _eventLogRepository.Add(obj);
                }
                catch (Exception) { throw; }
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            string deletedIds = string.Join(", ", entities.Where(x => x.Id > 0).Select(x => x.Id));

            await _repository.DeleteRangeAsync(entities, cancellationToken, saveNow);

            if (_logDeleteEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entities);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = deletedIds,
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.DeleteRange
                    };

                    await _eventLogRepository.AddAsync(obj, cancellationToken);
                }
                catch (Exception) { throw; }
            }
        }

        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _repository.FirstOrDefault(predicate);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _repository.FirstOrDefaultAsync(cancellationToken, predicate);
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _repository.GetAll(predicate);
        }

        public virtual TEntity GetById(params object[] ids)
        {
            return _repository.GetById(ids);
        }

        public virtual async ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            return (await _repository.GetByIdAsync(cancellationToken, ids));
        }

        public virtual TEntity Update(TEntity entity, bool saveNow = true)
        {
            if (_logUpdateEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entity);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = entity.Id.ToString(),
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.Update
                    };

                    _eventLogRepository.Add(obj);
                }
                catch (Exception) { throw; }
            }

            return _repository.Update(entity, saveNow);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken, bool saveNow = true)
        {
            if (_logUpdateEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entity);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = entity.Id.ToString(),
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.Update
                    };

                    await _eventLogRepository.AddAsync(obj, cancellationToken);
                }
                catch (Exception) { throw; }
            }

            return (await _repository.UpdateAsync(entity, cancellationToken, saveNow));
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            if (_logUpdateEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entities);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = string.Join(", ", entities.Where(x => x.Id > 0).Select(x => x.Id)),
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.UpdateRange
                    };

                    _eventLogRepository.Add(obj);
                }
                catch (Exception) { throw; }
            }

            _repository.UpdateRange(entities, saveNow);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool saveNow = true)
        {
            if (_logUpdateEvent)
            {
                try
                {
                    var strEntity = JsonConvert.SerializeObject(entities);

                    var obj = new EventLog()
                    {
                        Url = _url,
                        Method = _method,
                        IPAddress = _ipAddress,
                        EntityId = string.Join(", ", entities.Where(x => x.Id > 0).Select(x => x.Id)),
                        DataJson = strEntity,
                        EntityName = typeof(TEntity).Name,
                        HasError = false,
                        EventType = Domain.Enums.EventType.UpdateRange
                    };

                    await _eventLogRepository.AddAsync(obj, cancellationToken);
                }
                catch (Exception) { throw; }
            }

            await _repository.UpdateRangeAsync(entities, cancellationToken, saveNow);
        }
    }
}
