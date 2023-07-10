using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Feeding.Animals.Models;
using Rancho.Services.Feeding.Shared.Data;

namespace Rancho.Services.Feeding.Animals.Data;

public class AnimalDataTypeConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.ToTable(nameof(Animal).Pluralize().Underscore(), FeedingDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);

        builder.Property(x => x.Species)
            .HasMaxLength(40);
    }
}
