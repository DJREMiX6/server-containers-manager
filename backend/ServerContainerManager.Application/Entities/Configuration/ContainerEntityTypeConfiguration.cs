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

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.State)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(e => e.CreatedAt)
                .IsRequired();

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

            builder.HasMany(e => e.Namespaces)
                .WithMany();
        }
    }
}
