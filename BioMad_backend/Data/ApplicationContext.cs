using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleEntityConfiguration());
            builder.ApplyConfiguration(new GenderEntityConfiguration());
        }
    }
}