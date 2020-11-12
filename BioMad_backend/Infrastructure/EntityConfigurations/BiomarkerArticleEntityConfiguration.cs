using BioMad_backend.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class BiomarkerArticleEntityConfiguration : IEntityTypeConfiguration<BiomarkerArticle>
    {
        public void Configure(EntityTypeBuilder<BiomarkerArticle> builder)
        {
            builder.HasKey(x => new { x.TypeId, x.ArticleId, x.BiomarkerId });
            builder.HasIndex(x => new { x.TypeId, x.ArticleId, x.BiomarkerId }).IsUnique();
        }
    }
}