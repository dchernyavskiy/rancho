using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Rancho.Services.Identity.Shared.Data;

public class DbContextDesignFactory : DbContextDesignFactoryBase<IdentityContext>
{
    public DbContextDesignFactory()
        : base("PostgresOptions:ConnectionString") { }
}
