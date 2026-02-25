using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerContainerManager.Domain.Entities.Auth;

namespace ServerContainerManager.Application.Entities.Configuration
{
    internal class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasMany(e => e.Namespaces)
                .WithMany();
        }
    }
}
