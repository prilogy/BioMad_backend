using System.Collections.Generic;
using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class SocialAccountProviderEntityConfiguration : IEntityTypeConfiguration<SocialAccountProvider>
    {
        public void Configure(EntityTypeBuilder<SocialAccountProvider> builder)
        {
            builder.HasData(new List<SocialAccountProvider>
            {
                SocialAccountProvider.Google,
                SocialAccountProvider.Vk,
                SocialAccountProvider.Facebook
            });
        }
    }
}