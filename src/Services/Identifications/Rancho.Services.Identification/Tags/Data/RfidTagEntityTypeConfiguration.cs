using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Identification.Shared.Data;
using Rancho.Services.Identification.Tags.Models;

namespace Rancho.Services.Identification.Tags.Data;

public class RfidTagEntityTypeConfiguration : IEntityTypeConfiguration<RfidTag>
{
    public void Configure(EntityTypeBuilder<RfidTag> builder)
    {
        builder.ToTable(nameof(RfidTag).Pluralize().Underscore(), IdentificationDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
    }
}
