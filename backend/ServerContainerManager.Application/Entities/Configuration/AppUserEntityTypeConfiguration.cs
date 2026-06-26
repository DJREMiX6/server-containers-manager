using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Domain.Entities.Namespaces;

namespace ServerContainerManager.Application.Entities.Configuration
{
    internal class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(e => e.Namespaces)
                .WithMany(n => n.AssociatedUsers)
                .UsingEntity("AppUserNamespace",
                    r => r.HasOne(typeof(Namespace))
                          .WithMany()
                          .HasForeignKey("NamespacesId"),
                    l => l.HasOne(typeof(AppUser))
                          .WithMany()
                          .HasForeignKey("AppUsersId"));
        }
    }
}
