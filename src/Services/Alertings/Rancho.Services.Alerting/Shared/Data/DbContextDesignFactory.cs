using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Rancho.Services.Alerting.Shared.Data;

public class CatalogDbContextDesignFactory : DbContextDesignFactoryBase<AlertingDbContext>
{
    public CatalogDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
