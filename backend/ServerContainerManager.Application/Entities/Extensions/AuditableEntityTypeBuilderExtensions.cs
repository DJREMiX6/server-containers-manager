using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerContainerManager.Shared.Utils.Abstraction;

namespace ServerContainerManager.Application.Entities.Extensions
{
    internal static class AuditableEntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<T> ConfigureAuditableEntity<T>(this EntityTypeBuilder<T> builder) where T : class, IAuditableEntity
        {
            builder.ComplexProperty(e => e.Created, c =>
            {
                c.Property(p => p.At)
                    .HasColumnName("Created_At")
                    .IsRequired();

                c.ComplexProperty(c => c.By, b =>
                {
                    b.Property(p => p.Id)
                        .HasColumnName("Created_By_Id")
                        .IsRequired(false);

                    b.Property(p => p.ActorType)
                        .HasColumnName("Created_By_ActorType")
                        .IsRequired()
                        .HasConversion<string>();
                });
            });

            builder.ComplexProperty(e => e.Updated, c =>
            {
                c.Property(p => p.At)
                    .HasColumnName("Updated_At")
                    .IsRequired();

                c.ComplexProperty(c => c.By, b =>
                {
                    b.Property(p => p.Id)
                        .HasColumnName("Updated_By_Id")
                        .IsRequired(false);

                    b.Property(p => p.ActorType)
                        .HasColumnName("Updated_By_ActorType")
                        .IsRequired()
                        .HasConversion<string>();
                });
            });

            return builder;
        }
    }
}
