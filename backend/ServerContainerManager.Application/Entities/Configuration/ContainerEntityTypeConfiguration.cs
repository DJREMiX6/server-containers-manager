using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerContainerManager.Domain.Entities.Containers;
using ServerContainerManager.Domain.Entities.Namespaces;
using ServerContainerManager.Application.Entities.Extensions;

namespace ServerContainerManager.Application.Entities.Configuration
{
    internal class ContainerEntityTypeConfiguration : IEntityTypeConfiguration<Container>
    {
        public void Configure(EntityTypeBuilder<Container> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.State)
                .IsRequired()
                .HasConversion<string>();

            builder.OwnsMany(e => e.Labels, l =>
            {
                l.ToTable("ContainerLabels");
                l.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                l.HasKey("Id");

                l.WithOwner().HasForeignKey("ContainerId");

                l.Property(e => e.Key)
                    .IsRequired();

                l.Property(e => e.Value)
                    .IsRequired();
            });

            builder.OwnsMany(e => e.Ports, p =>
            {
                p.ToTable("ContainerPorts"); 
                
                p.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                p.HasKey("Id");

                p.WithOwner().HasForeignKey("ContainerId");

                p.Property(e => e.Public)
                    .IsRequired();

                p.Property(e => e.Private)
                    .IsRequired();
            });

            builder.ConfigureAuditableEntity();

            builder.HasMany(e => e.Namespaces)
                .WithMany(n => n.AssociatedContainers)
                .UsingEntity("ContainerNamespace",
                    r => r.HasOne(typeof(Namespace)).WithMany().HasForeignKey("NamespacesId"),
                    l => l.HasOne(typeof(Container)).WithMany().HasForeignKey("ContainerId"));
        }
    }
}
