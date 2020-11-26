using BioMad_backend.Entities;
using BioMad_backend.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class MemberBiomarkerReferenceEntityConfiguration : IEntityTypeConfiguration<MemberBiomarkerReference>
    {
        public void Configure(EntityTypeBuilder<MemberBiomarkerReference> builder)
        {
            builder.HasKey(x => new { x.MemberId, x.BiomarkerReferenceId });
            builder.HasIndex(x => new { x.MemberId, x.BiomarkerReferenceId }).IsUnique();
        }
    }
}