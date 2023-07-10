using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Management.Farmers.Models;

namespace Rancho.Services.Management.Farmers.Data;

public class FarmerDataTypeConfiguration : IEntityTypeConfiguration<Farmer>
{
    public void Configure(EntityTypeBuilder<Farmer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);

        builder.Property(x => x.FirstName).HasMaxLength(30);
        builder.Property(x => x.LastName).HasMaxLength(30);

        builder.HasOne(x => x.Farm)
            .WithMany(x => x.Farmers)
            .HasForeignKey(x => x.FarmId);

        builder.HasMany(x => x.Works)
            .WithOne(x => x.Farmer)
            .HasForeignKey(x => x.FarmerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
