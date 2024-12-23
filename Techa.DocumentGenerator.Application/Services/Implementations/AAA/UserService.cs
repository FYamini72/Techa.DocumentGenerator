using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Techa.DocumentGenerator.Application.Repositories;
using Techa.DocumentGenerator.Application.Services.Interfaces.AAA;
using Techa.DocumentGenerator.Domain.Entities;
using Techa.DocumentGenerator.Domain.Entities.AAA;

namespace Techa.DocumentGenerator.Application.Services.Implementations.AAA
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IBaseRepository<User> _repository;

        public UserService(IBaseRepository<User> repository
            , IBaseRepository<EventLog> eventLogRepository
            , IConfiguration configuration
            , IHttpContextAccessor httpContext) : base(repository, eventLogRepository, configuration, httpContext)
        {
            _repository = repository;
        }
    }
}
