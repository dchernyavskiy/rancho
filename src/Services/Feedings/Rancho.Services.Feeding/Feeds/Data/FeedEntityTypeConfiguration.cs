using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Shared.Data;

namespace Rancho.Services.Feeding.Feeds.Data;

public class FeedEntityTypeConfiguration : IEntityTypeConfiguration<Feed>
{
    public void Configure(EntityTypeBuilder<Feed> builder)
    {
        builder.ToTable(nameof(Feed).Pluralize().Underscore(), FeedingDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Type).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.OwnsOne(x => x.Nutrition);

        builder.HasOne(x => x.Farm)
            .WithMany(x => x.Feeds)
            .HasForeignKey(x => x.FarmId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

