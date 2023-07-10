using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.Works.Enums;
using Rancho.Services.Management.Works.Models;

namespace Rancho.Services.Management.Works.Data;

public class WorkDataTypeConfiguration : IEntityTypeConfiguration<Work>
{
    public void Configure(EntityTypeBuilder<Work> builder)
    {
        builder.ToTable(nameof(Work).Pluralize().Underscore(), ManagementDbContext.DefaultSchema);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Start).IsRequired();
        builder.Property(x => x.End).IsRequired();
        builder.Property(x => x.FarmerId).IsRequired();

        builder.Property(x => x.Status).HasConversion(x => x.ToString(), v => Enum.Parse<WorkStatus>(v));
    }
}
