using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Animals.Models;
using Rancho.Services.Feeding.Farms.Models;
using Rancho.Services.Feeding.FeedPlans.Models;
using Rancho.Services.Feeding.Feeds.Models;

namespace Rancho.Services.Feeding.Shared.Contracts;

public interface IFeedingDbContext
{
    DbSet<Animal> Animals { get; }
    DbSet<Farm> Farms { get; }
    DbSet<Feed> Feeds { get; }
    DbSet<FeedPlan> FeedPlans { get; }

    DbSet<TEntity> Set<TEntity>()
    where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
