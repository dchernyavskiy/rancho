using System.Reflection;
using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Management.Works.Models;

namespace Rancho.Services.Management.Shared.Data;

public class ManagementDbContext : EfDbContextBase, IManagementDbContext
{
    public const string DefaultSchema = "management";

    public ManagementDbContext(DbContextOptions<ManagementDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Animal> Animals => Set<Animal>();
    public DbSet<Farm> Farms => Set<Farm>();
    public DbSet<Farmer> Farmers => Set<Farmer>();
    public DbSet<Work> Works => Set<Work>();
}
