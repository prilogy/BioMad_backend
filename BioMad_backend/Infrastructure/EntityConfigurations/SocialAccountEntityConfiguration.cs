using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class SocialAccountEntityConfiguration : IEntityTypeConfiguration<SocialAccount>
    {
        public void Configure(EntityTypeBuilder<SocialAccount> builder)
        {
            builder.HasIndex(x => new { x.UserId, x.ProviderId }).IsUnique();
        }
    }
}