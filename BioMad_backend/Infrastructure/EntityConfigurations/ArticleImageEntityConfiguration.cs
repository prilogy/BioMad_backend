using BioMad_backend.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class ArticleImageEntityConfiguration : IEntityTypeConfiguration<ArticleImage>
    {
        public void Configure(EntityTypeBuilder<ArticleImage> builder)
        {
            builder.HasKey(x => new { x.ImageId, x.ArticleId });
            builder.HasIndex(x => new { x.ImageId, x.ArticleId }).IsUnique();
        }
    }
}