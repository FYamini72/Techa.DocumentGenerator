using Mapster;
using Techa.DocumentGenerator.Application.Dtos.AAA;
using Techa.DocumentGenerator.Domain.Entities.AAA;

namespace Techa.DocumentGenerator.API.Mapping
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<UserRole, UserRoleDisplayDto>
                .NewConfig()
                .Map(destination => destination.RoleTitle, source => source.Role == null ? string.Empty : source.Role.Title)
                ;

            TypeAdapterConfig<User, UserDisplayDto>
                .NewConfig()
                .Map(destination => destination.AttachmentUrl, source => source.Profile != null ? $"/staticfiles/{source.Profile.FileName}" : "")
                ;
        }
    }
}
