using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;

namespace Techa.DocumentGenerator.Domain.EntitiesConfigurations.DbInfo
{
    public class StoredProcedureParameterConfiguration : IEntityTypeConfiguration<StoredProcedureParameter>
    {
        public void Configure(EntityTypeBuilder<StoredProcedureParameter> builder)
        {
            builder.Property(x => x.ParameterName).HasMaxLength(250).IsRequired();
            builder.Property(x => x.ParameterType).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.Title).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.Alias).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.DefaultValue).HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.MinValue).HasMaxLength(150).IsRequired(false);
            builder.Property(x => x.MaxValue).HasMaxLength(150).IsRequired(false);
            builder.Property(x => x.MinLength).IsRequired(false);
            builder.Property(x => x.MaxLength).IsRequired(false);
            builder.Property(x => x.NullableOption).IsRequired();
        }
    }
}
