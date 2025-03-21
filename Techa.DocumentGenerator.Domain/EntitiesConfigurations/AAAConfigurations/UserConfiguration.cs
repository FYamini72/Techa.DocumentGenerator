using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Techa.DocumentGenerator.Domain.Entities.AAA;

namespace Techa.DocumentGenerator.Domain.EntitiesConfigurations.AAAConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId);

            builder.HasOne(u => u.Profile).WithMany().HasForeignKey(u => u.ProfileId);
            builder.HasOne(u => u.Project).WithMany().HasForeignKey(u => u.ProjectId);

            builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
        }
    }
}
