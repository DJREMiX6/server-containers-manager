using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerContainerManager.Domain.Entities.Containers;

namespace ServerContainerManager.Application.Entities.Configuration
{
    internal class ContainerEntityTypeConfiguration : IEntityTypeConfiguration<Container>
    {
        public void Configure(EntityTypeBuilder<Container> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(e => e.Namespaces)
                .WithMany();
        }
    }
}
