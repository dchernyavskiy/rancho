using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Management.Shared.Data;

namespace Rancho.Services.Management.Animals.Data;

public class FarmDataTypeConfiguration : IEntityTypeConfiguration<Farms.Models.Farm>
{
    public void Configure(EntityTypeBuilder<Farms.Models.Farm> builder)
    {
        builder.ToTable(nameof(Farms.Models.Farm).Pluralize().Underscore(), ManagementDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(100);

        builder.HasMany(x => x.Farmers)
            .WithOne(x => x.Farm)
            .HasForeignKey(x => x.FarmId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Animals)
            .WithOne(x => x.Farm)
            .HasForeignKey(x => x.FarmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
