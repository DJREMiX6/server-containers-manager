using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Domain.Entities.Namespaces;

namespace ServerContainerManager.Application.Entities
{
    internal class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, AppRole, Guid>(options)
    {
        public DbSet<Namespace> Namespaces { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
