using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.Postgres;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rancho.Services.Feeding.Shared.Contracts;
using Rancho.Services.Feeding.Shared.Data;

namespace Rancho.Services.Feeding.Extensions.WebApplicationBuilderExtensions;

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
            services.AddDbContext<IFeedingDbContext, FeedingDbContext>(
                options => options.UseInMemoryDatabase("Rancho.Services.Feeding")
            );

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<FeedingDbContext>()!);
        }
        else
        {
            services.AddPostgresDbContext<FeedingDbContext>();
        }

        services.AddScoped<IFeedingDbContext, FeedingDbContext>();
    }

    private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddMongoDbContext<>();
    }
}
