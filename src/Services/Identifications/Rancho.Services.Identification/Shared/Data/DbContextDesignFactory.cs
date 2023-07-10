using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Rancho.Services.Identification.Shared.Data;

public class CatalogDbContextDesignFactory : DbContextDesignFactoryBase<IdentificationDbContext>
{
    public CatalogDbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
