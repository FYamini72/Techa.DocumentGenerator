using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;

namespace Techa.DocumentGenerator.Domain.EntitiesConfigurations.DbInfo
{
    public class StoredProcedureConfiguration : IEntityTypeConfiguration<StoredProcedure>
    {
        public void Configure(EntityTypeBuilder<StoredProcedure> builder)
        {
            builder.Property(x => x.ProcedureName).HasMaxLength(250).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.Alias).HasMaxLength(250).IsRequired(false);
        }
    }
}
