using BioMad_backend.Entities;
using BioMad_backend.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class SharedEntityConfiguration : IEntityTypeConfiguration<Shared>
    {
        public void Configure(EntityTypeBuilder<Shared> builder)
        {
            builder.Property(e => e.BiomarkerIds)
                .HasConversion(ValueConverters.StringToIntList);
            
            builder.Property(e => e.BiomarkerIds).Metadata
                .SetValueComparer(ValueConverters.StringToIntListComparer);
        }
    }
}