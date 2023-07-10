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
using Rancho.Services.Alerting.Alerts;
using Rancho.Services.Alerting.Farms;

namespace Rancho.Services.Alerting.Shared.Extensions.WebApplicationBuilderExtensions;

public static partial class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddCore(builder.Configuration);

        builder.Services.AddCustomJwtAuthentication(builder.Configuration);
        builder.Services.AddCustomAuthorization(
            rolePolicies: new List<RolePolicy>
                          {
                              new(AlertingConstants.Role.Admin, new List<string> {AlertingConstants.Role.Admin}),
                              new(AlertingConstants.Role.User, new List<string> {AlertingConstants.Role.User})
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

        DotNetEnv.Env.TraversePath().Load();

        var configFolder = builder.Configuration.GetValue<string>("ConfigurationFolder") ?? "config-files/";
        builder.Configuration.AddKeyPerFile(configFolder, true, true);

        // https://www.michaco.net/blog/EnvironmentVariablesAndConfigurationInASPNETCoreApps#environment-variables-and-configuration
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0#non-prefixed-environment-variables
        builder.Configuration.AddEnvironmentVariables("Rancho_catalogs_env_");

        builder.AddCustomVersioning();

        builder.AddCustomSwagger(typeof(AlertingAssemblyInfo).Assembly);

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
                busFactoryConfigurator.AddAlertEndpoints(busRegistrationContext);
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

        builder.AddCustomCaching();

        return builder;
    }
}
