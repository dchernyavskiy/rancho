using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Feeding.Shared.Data;
using Rancho.Services.Feeding.Farms.Models;

namespace Rancho.Services.Feeding.Farms.Data;

public class FarmEntityTypeConfiguration : IEntityTypeConfiguration<Farm>
{
    public void Configure(EntityTypeBuilder<Farm> builder)
    {
        builder.ToTable(nameof(Farm).Pluralize().Underscore(), FeedingDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);

        builder.HasMany(x => x.Animals)
            .WithOne(x => x.Farm)
            .HasForeignKey(x => x.FarmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
