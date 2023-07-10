using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Asn1.X509.SigI;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Data;

namespace Rancho.Services.Management.Animals.Data;

public class AnimalDataTypeConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.ToTable(nameof(Animal).Pluralize().Underscore(), ManagementDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);
        builder.HasIndex(nameof(Animal.Id), nameof(Animal.EarTagNumber), nameof(Animal.FarmId)).IsUnique();

        builder.Property(x => x.Species)
            .HasMaxLength(100);

        builder.HasOne(x => x.Farm)
            .WithMany(x => x.Animals)
            .HasForeignKey(x => x.FarmId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
