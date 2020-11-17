using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using BioMad_backend.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Culture> Cultures { get; set; }

        public DbSet<SocialAccount> SocialAccounts { get; set; }
        public DbSet<SocialAccountProvider> SocialAccountProviders { get; set; }
        public DbSet<Member> Members { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ConfirmationCode> ConfirmationCodes { get; set; }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitTransfer> UnitTransfers { get; set; }
        
        // Biomarker system
        public DbSet<Biomarker> Biomarkers { get; set; }
        public DbSet<BiomarkerType> BiomarkerTypes { get; set; }
        public DbSet<BiomarkerArticleType> BiomarkerArticleTypes { get; set; }
        public DbSet<BiomarkerArticle> BiomarkerArticles { get; set; }
        public DbSet<BiomarkerUnit> BiomarkerUnits { get; set; }
        
        public DbSet<BiomarkerReference> BiomarkerReferences { get; set; }
        public DbSet<BiomarkerReferenceConfig> BiomarkerReferenceConfigs { get; set; }
        public DbSet<BiomarkerReferenceConfigDependency> BiomarkerReferenceConfigDependencies { get; set; }
        public DbSet<BiomarkerReferenceConfigDependencyRange> BiomarkerReferenceConfigDependencyRanges { get; set; }
        public DbSet<BiomarkerReferenceUser> BiomarkerReferenceUsers { get; set; }
        
        public DbSet<City> Cities { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<UserAnalysis> UserAnalyzes { get; set; }
        public DbSet<UserBiomarker> UserBiomarkers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleEntityConfiguration());
            builder.ApplyConfiguration(new GenderEntityConfiguration());
            builder.ApplyConfiguration(new SocialAccountProviderEntityConfiguration());
            builder.ApplyConfiguration(new SocialAccountEntityConfiguration());
            builder.ApplyConfiguration(new CultureEntityConfiguration());
            builder.ApplyConfiguration(new UnitTransferEntityConfiguration());
            
            builder.ApplyConfiguration(new CategoryBiomarkerEntityConfiguration());
            builder.ApplyConfiguration(new BiomarkerArticleEntityConfiguration());
            builder.ApplyConfiguration(new BiomarkerUnitEntityConfiguration());
            builder.ApplyConfiguration(new BiomarkerArticleTypeEntityConfiguration());
            builder.ApplyConfiguration(new BiomarkerReferenceUserEntityConfiguration());
        }
        
        public DbSet<UserAnalysis> UserAnalysis { get; set; }
    }
}