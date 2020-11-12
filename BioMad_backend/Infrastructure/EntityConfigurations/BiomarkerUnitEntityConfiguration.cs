using BioMad_backend.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class BiomarkerUnitEntityConfiguration : IEntityTypeConfiguration<BiomarkerUnit>
    {
        public void Configure(EntityTypeBuilder<BiomarkerUnit> builder)
        {
            builder.HasKey(x => new { x.BiomarkerId, x.UnitId });
            builder.HasKey(x => new { x.BiomarkerId, x.UnitId });
        }
    }
}