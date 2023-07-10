using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Works.Models;

namespace Rancho.Services.Management.Shared.Contracts;

public interface IManagementDbContext
{
    DbSet<Animal> Animals { get; }
    DbSet<Farm> Farms { get; }
    DbSet<Farmer> Farmers { get; }
    DbSet<Work> Works { get; }


    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
