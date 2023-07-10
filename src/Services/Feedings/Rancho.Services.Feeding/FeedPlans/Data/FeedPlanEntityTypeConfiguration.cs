using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Feeding.FeedPlans.Models;
using Rancho.Services.Feeding.Shared.Data;

namespace Rancho.Services.Feeding.FeedPlans.Data;

public class FeedPlanEntityTypeConfiguration : IEntityTypeConfiguration<FeedPlan>
{
    public void Configure(EntityTypeBuilder<FeedPlan> builder)
    {
        builder.ToTable(nameof(FeedPlan).Pluralize().Underscore(), FeedingDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.AnimalId);


        builder.Property(x => x.WeightDispensed)
            .HasColumnType("decimal(18,2)");
        builder.Property(x => x.WeightEaten)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Feed)
            .WithMany(x => x.FeedPlans)
            .HasForeignKey(x => x.FeedId);
        builder.HasOne(x => x.Animal)
            .WithMany(x => x.FeedPlans)
            .HasForeignKey(x => x.AnimalId);
    }
}
