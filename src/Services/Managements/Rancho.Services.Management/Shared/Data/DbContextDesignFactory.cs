using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Rancho.Services.Management.Shared.Data;

public class CatalogDbContextDesignFactory : DbContextDesignFactoryBase<ManagementDbContext>
{
    public CatalogDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
