using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;

namespace Techa.DocumentGenerator.Domain.EntitiesConfigurations.BaseInfo
{
    partial class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.DbName).HasMaxLength(250).IsRequired();
            builder.Property(x => x.DbServerName).HasMaxLength(250).IsRequired();
            builder.Property(x => x.DbUserName).HasMaxLength(250).IsRequired();
            builder.Property(x => x.DbPassword).HasMaxLength(250).IsRequired();
            builder.Property(x => x.ConnectionString).HasMaxLength(1500).IsRequired(false);
        }
    }
}
