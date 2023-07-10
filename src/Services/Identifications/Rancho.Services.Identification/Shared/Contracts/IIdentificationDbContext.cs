using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Farms.Models;
using Rancho.Services.Identification.Tags.Models;

namespace Rancho.Services.Identification.Shared.Contracts;

public interface IIdentificationDbContext
{
    DbSet<Animal> Animals { get; }
    DbSet<RfidTag> RfidTags { get; }
    DbSet<Farm> Farms { get; }

    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
