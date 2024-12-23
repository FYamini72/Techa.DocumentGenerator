using Techa.DocumentGenerator.Domain.Entities.AAA;

namespace Techa.DocumentGenerator.Application.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> Generate(User user);
    }
}
