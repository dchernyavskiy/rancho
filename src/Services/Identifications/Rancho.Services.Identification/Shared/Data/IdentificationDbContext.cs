using System.Reflection;
using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Shared.Contracts;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Farms.Models;
using Rancho.Services.Identification.Tags.Models;

namespace Rancho.Services.Identification.Shared.Data;

public class IdentificationDbContext : EfDbContextBase, IIdentificationDbContext
{
    public const string DefaultSchema = "identification";

    public IdentificationDbContext(DbContextOptions<IdentificationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }


    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<RfidTag> RfidTags => Set<RfidTag>();
    public DbSet<Farm> Farms => Set<Farm>();
}
