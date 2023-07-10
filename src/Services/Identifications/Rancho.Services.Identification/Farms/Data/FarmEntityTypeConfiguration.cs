using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Identification.Farms.Models;
using Rancho.Services.Identification.Shared.Data;

namespace Rancho.Services.Identification.Farms.Data;

public class FarmEntityTypeConfiguration : IEntityTypeConfiguration<Farm>
{
    public void Configure(EntityTypeBuilder<Farm> builder)
    {
        builder.ToTable(nameof(Farm).Pluralize().Underscore(), IdentificationDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);

        builder.HasMany(x => x.Animals)
            .WithOne(x => x.Farm)
            .HasForeignKey(x => x.FarmId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RfidTags)
            .WithOne(x => x.Farm)
            .HasForeignKey(x => x.FarmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
