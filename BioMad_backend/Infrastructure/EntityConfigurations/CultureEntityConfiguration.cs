using System.Collections.Generic;
using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class CultureEntityConfiguration : IEntityTypeConfiguration<Culture>
    {
        public void Configure(EntityTypeBuilder<Culture> builder)
        {
            builder.HasData(new List<Culture>
            {
                Culture.En,
                Culture.Ru
            });
        }
    }
}