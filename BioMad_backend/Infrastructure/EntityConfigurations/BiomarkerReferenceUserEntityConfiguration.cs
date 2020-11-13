using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class BiomarkerReferenceUserEntityConfiguration : IEntityTypeConfiguration<BiomarkerReferenceUser>
    {
        public void Configure(EntityTypeBuilder<BiomarkerReferenceUser> builder)
        {
            builder.HasKey(x => new { x.UserId, x.BiomarkerReferenceId });
            builder.HasIndex(x => new { x.UserId, x.BiomarkerReferenceId }).IsUnique();
        }
    }
}