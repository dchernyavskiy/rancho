using System.Reflection;
using Ardalis.GuardClauses;
using BuildingBlocks.Caching;
using BuildingBlocks.Caching.Behaviours;
using BuildingBlocks.Core.Mapping;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Core.Registrations;
using BuildingBlocks.Core.Web.Extenions;
using BuildingBlocks.Email;
using BuildingBlocks.HealthCheck;
using BuildingBlocks.Integration.MassTransit;
using BuildingBlocks.Logging;
using BuildingBlocks.Messaging.Persistence.Postgres.Extensions;
using BuildingBlocks.OpenTelemetry;
using BuildingBlocks.Persistence.EfCore.Postgres;
using BuildingBlocks.Security.Extensions;
using BuildingBlocks.Security.Jwt;
using BuildingBlocks.Swagger;
using BuildingBlocks.Validation;
using BuildingBlocks.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Rancho.Services.Identification.Animals;
using Rancho.Services.Identification.Animals.Features.CreatingAnimal.v1.Events.Integration.External;
using Rancho.Services.Identification.Animals.Features.DeletingAnimal.v1.Events.Integration.External;
using Rancho.Services.Identification.Animals.Features.UpdatingAnimal.v1.Events.Integration.External;
using Rancho.Services.Identification.Farms;
using Rancho.Services.Identification.Farms.Features.CreatingFarm.v1.Events.Integration.External;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Identification.Shared.Extensions.WebApplicationBuilderExtensions;

public static partial class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddCore(builder.Configuration);

        builder.Services.AddCustomJwtAuthentication(builder.Configuration);
        builder.Services.AddCustomAuthorization(
            rolePolicies: new List<RolePolicy>
                          {
                              new(
                                  IdentificationConstants.Role.Admin,
                                  new List<string> {IdentificationConstants.Role.Admin}),
                              new(
                                  IdentificationConstants.Role.User,
                                  new List<string> {IdentificationConstants.Role.User})
                          });

        builder.Services.AddEmailService(builder.Configuration);
        builder.Services.AddCqrs(
            pipelines: new[]
                       {
                           typeof(RequestValidationBehavior<,>),
                           typeof(StreamRequestValidationBehavior<,>),
                           typeof(StreamLoggingBehavior<,>),
                           typeof(StreamCachingBehavior<,>),
                           typeof(LoggingBehavior<,>),
                           typeof(CachingBehavior<,>),
                           typeof(InvalidateCachingBehavior<,>),
                           typeof(EfTxBehavior<,>)
                       });

        // https://github.com/tonerdo/dotnet-env
        DotNetEnv.Env.TraversePath().Load();

        // https://www.thorsten-hans.com/hot-reload-net-configuration-in-kubernetes-with-configmaps/
        // https://bartwullems.blogspot.com/2021/03/kubernetesoverride-appsettingsjson-file.html
        // if we need to reload app by change settings, we can use volume map for watching our new setting from config-files folder in the the volume or change appsettings.json file in the volume map
        var configFolder = builder.Configuration.GetValue<string>("ConfigurationFolder") ?? "config-files/";
        builder.Configuration.AddKeyPerFile(configFolder, true, true);

        // https://www.michaco.net/blog/EnvironmentVariablesAndConfigurationInASPNETCoreApps#environment-variables-and-configuration
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0#non-prefixed-environment-variables
        builder.Configuration.AddEnvironmentVariables("Rancho_catalogs_env_");

        builder.AddCustomVersioning();

        builder.AddCustomSwagger(typeof(IdentificationAssemblyInfo).Assembly);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddPostgresMessagePersistence(builder.Configuration);

        builder.AddCompression();
        builder.AddCustomProblemDetails();

        builder.AddCustomSerilog();

        builder.AddCustomOpenTelemetry();

        // https://blog.maartenballiauw.be/post/2022/09/26/aspnet-core-rate-limiting-middleware.html
        builder.AddCustomRateLimit();

        builder.AddCustomMassTransit(
            (busRegistrationContext, busFactoryConfigurator) =>
            {
                busFactoryConfigurator.AddAnimalPublishers();
                busFactoryConfigurator.AddAnimalEndpoints(busRegistrationContext);
                busFactoryConfigurator.AddFarmEndpoints(busRegistrationContext);
            });

        if (builder.Environment.IsTest() == false)
        {
            builder.AddCustomHealthCheck(
                healthChecksBuilder =>
                {
                    var postgresOptions = builder.Configuration.BindOptions<PostgresOptions>(nameof(PostgresOptions));
                    var rabbitMqOptions = builder.Configuration.BindOptions<RabbitMqOptions>(nameof(RabbitMqOptions));

                    Guard.Against.Null(postgresOptions, nameof(postgresOptions));
                    Guard.Against.Null(rabbitMqOptions, nameof(rabbitMqOptions));

                    healthChecksBuilder
                        .AddNpgSql(
                            postgresOptions.ConnectionString,
                            name: "CatalogsService-Postgres-Check",
                            tags: new[]
                                  {
                                      "postgres",
                                      "infra",
                                      "database",
                                      "catalogs-service",
                                      "live",
                                      "ready"
                                  })
                        .AddRabbitMQ(
                            rabbitMqOptions.ConnectionString,
                            name: "CatalogsService-RabbitMQ-Check",
                            timeout: TimeSpan.FromSeconds(3),
                            tags: new[]
                                  {
                                      "rabbitmq",
                                      "infra",
                                      "bus",
                                      "catalogs-service",
                                      "live",
                                      "ready"
                                  });
                });
        }

        builder.Services.AddCustomValidators(Assembly.GetExecutingAssembly());
        builder.AddCustomAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddScoped<ISecurityContextAccessor, SecurityContextAccessor>();
        builder.AddCustomCaching();

        return builder;
    }
}
