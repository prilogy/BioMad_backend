using BioMad_backend.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class UnitGroupUnitEntityConfiguration : IEntityTypeConfiguration<UnitGroupUnit>
    {
        public void Configure(EntityTypeBuilder<UnitGroupUnit> builder)
        {
            builder.HasKey(x => new { x.UnitGroupId, x.UnitId });
            builder.HasKey(x => new { x.UnitGroupId, x.UnitId });
        }
    }
}