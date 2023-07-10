using System.Reflection;
using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Animals.Models;
using Rancho.Services.Feeding.Farms.Models;
using Rancho.Services.Feeding.FeedPlans.Models;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Shared.Contracts;

namespace Rancho.Services.Feeding.Shared.Data;

public class FeedingDbContext : EfDbContextBase, IFeedingDbContext
{
    public const string DefaultSchema = "feeding";

    public FeedingDbContext(DbContextOptions<FeedingDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }


    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Farm> Farms => Set<Farm>();
    public DbSet<Feed> Feeds => Set<Feed>();
    public DbSet<FeedPlan> FeedPlans => Set<FeedPlan>();
}
