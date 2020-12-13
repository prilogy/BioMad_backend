using System.Collections.Generic;
using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class BiomarkerArticleTypeEntityConfiguration : IEntityTypeConfiguration<BiomarkerArticleType>
    {
        public void Configure(EntityTypeBuilder<BiomarkerArticleType> builder)
        {
            builder.HasData(new List<BiomarkerArticleType>
            {
                BiomarkerArticleType.Decrease,
                BiomarkerArticleType.Increase,
                BiomarkerArticleType.Decreased,
                BiomarkerArticleType.Increased
            });
        }
    }
}