using BioMad_backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioMad_backend.Infrastructure.EntityConfigurations
{
    public class UnitTransferEntityConfiguration : IEntityTypeConfiguration<UnitTransfer>
    {
        public void Configure(EntityTypeBuilder<UnitTransfer> builder)
        {
            builder.HasOne(x => x.UnitA)
                .WithMany(x => x.Transfers)
                .HasForeignKey(x => x.UnitAId);

            builder.HasOne(x => x.UnitB)
                .WithMany(x => x.Transfers)
                .HasForeignKey(x => x.UnitBId);
        }
    }
}