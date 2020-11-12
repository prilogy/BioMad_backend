using BioMad_backend.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class CategoryBiomarkerEntityConfiguration : IEntityTypeConfiguration<CategoryBiomarker>
    {
        public void Configure(EntityTypeBuilder<CategoryBiomarker> builder)
        {
            builder.HasKey(x => new { x.BiomarkerId, x.CategoryId });
            builder.HasIndex(x => new { x.BiomarkerId, x.CategoryId }).IsUnique();
        }
    }
}