using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Identification.Shared.Data;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Tags.Models;

namespace Rancho.Services.Identification.Animals.Data;

public class AnimalDataTypeConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.ToTable(nameof(Animal).Pluralize().Underscore(), IdentificationDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);

        builder.HasOne(x => x.RfidTag)
            .WithOne(x => x.Animal)
            .HasForeignKey<RfidTag>(x => x.AnimalId);


        builder.Property(x => x.Species)
            .HasMaxLength(40);
    }
}
