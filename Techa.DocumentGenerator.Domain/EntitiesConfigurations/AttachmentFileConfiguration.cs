using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Techa.DocumentGenerator.Domain.Entities;

namespace Techa.DocumentGenerator.Domain.EntitiesConfigurations
{
    public class AttachmentFileConfiguration : IEntityTypeConfiguration<AttachmentFile>
    {
        public void Configure(EntityTypeBuilder<AttachmentFile> builder)
        {
            builder.Property(x => x.FileName).HasMaxLength(100);
        }
    }
}
