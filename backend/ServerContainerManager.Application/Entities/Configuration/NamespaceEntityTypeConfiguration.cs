using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerContainerManager.Domain.Entities.Namespaces;

namespace ServerContainerManager.Application.Entities.Configuration
{
    internal class NamespaceEntityTypeConfiguration : IEntityTypeConfiguration<Namespace>
    {
        public void Configure(EntityTypeBuilder<Namespace> builder)
        {
            builder.ToTable("Namespaces");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);
            builder.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
