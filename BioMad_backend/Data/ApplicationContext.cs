using BioMad_backend.Entities;
using BioMad_backend.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        
        public DbSet<SocialAccount> SocialAccounts { get; set; }
        public DbSet<SocialAccountProvider> SocialAccountProviders { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ConfirmationCode> ConfirmationCodes { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleEntityConfiguration());
            builder.ApplyConfiguration(new GenderEntityConfiguration());
            builder.ApplyConfiguration(new SocialAccountProviderEntityConfiguration());
            builder.ApplyConfiguration(new SocialAccountEntityConfiguration());
            builder.ApplyConfiguration(new CultureEntityConfiguration());
        }
    }
}