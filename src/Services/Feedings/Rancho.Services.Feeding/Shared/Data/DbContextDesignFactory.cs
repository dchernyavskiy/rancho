using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Rancho.Services.Feeding.Shared.Data;

public class CatalogDbContextDesignFactory : DbContextDesignFactoryBase<FeedingDbContext>
{
    public CatalogDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
