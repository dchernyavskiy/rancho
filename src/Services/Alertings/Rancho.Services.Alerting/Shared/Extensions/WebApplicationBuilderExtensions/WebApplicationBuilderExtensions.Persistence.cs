using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.Postgres;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rancho.Services.Alerting.Shared.Contracts;
using Rancho.Services.Alerting.Shared.Data;

namespace Rancho.Services.Alerting.Extensions.WebApplicationBuilderExtensions;

public static partial class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddStorage(this WebApplicationBuilder builder)
    {
        AddPostgresWriteStorage(builder.Services, builder.Configuration);
        AddMongoReadStorage(builder.Services, builder.Configuration);

        return builder;
    }

    private static void AddPostgresWriteStorage(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("PostgresOptions:UseInMemory"))
        {
            services.AddDbContext<IAlertingDbContext, AlertingDbContext>(
                options => options.UseInMemoryDatabase("Rancho.Services.Alerting")
            );

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<AlertingDbContext>()!);
        }
        else
        {
            services.AddPostgresDbContext<AlertingDbContext>();
        }

        services.AddScoped<IAlertingDbContext, AlertingDbContext>();
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddMongoDbContext<>();
    }
}
