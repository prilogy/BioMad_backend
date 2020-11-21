using System.Collections.Generic;
using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class GenderEntityConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasData(new List<Gender>
            {
                Gender.Male,
                Gender.Female,
                Gender.Neutral
            });

            builder.HasMany(x => x.Translations)
                .WithOne(x => x.BaseEntity)
                .HasForeignKey(x => x.BaseEntityId);
        }
    }
}