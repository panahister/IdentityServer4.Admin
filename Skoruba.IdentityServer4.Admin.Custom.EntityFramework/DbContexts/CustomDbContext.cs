using Microsoft.EntityFrameworkCore;
using Custom.EntityFramework.Constants;
using Custom.EntityFramework.Entities;
using Custom.EntityFramework.Interfaces;
namespace Custom.EntityFramework.DbContexts
{
    public class CustomDbContext : DbContext, ICustomDbContext
    {

        public CustomDbContext(DbContextOptions<CustomDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Person> Pepole { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureLogContext(builder);
        }

        private void ConfigureLogContext(ModelBuilder builder)
        {
            builder.Entity<Resource>(log =>
            {
                log.ToTable(TableConsts.Resource);
                log.HasKey(x => x.Id);
            });

            builder.Entity<Permission>(log =>
            {
                log.ToTable(TableConsts.Permission);
                log.HasKey(x => x.Id);
            });
            builder.Entity<Person>(log =>
            {
                log.ToTable(TableConsts.Person);
                log.HasKey(x => x.Id);
            });
        }
    }
}
