using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Rancho.Services.Management.Shared.Extensions.WebApplicationBuilderExtensions;
using Rancho.Services.Management.Shared.Extensions.WebApplicationExtensions;
using Rancho.Services.Management.Extensions.WebApplicationBuilderExtensions;

namespace Rancho.Services.Management.Shared;

public class SharedModulesConfiguration : ISharedModulesConfiguration
{
    public const string ManagementModulePrefixUri = "api/v{version:apiVersion}/management";
    public const string ManagementModulePrefixUriV1 = "api/v1/management";

    public IEndpointRouteBuilder MapSharedModuleEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/",
                (HttpContext context) =>
                {
                    var requestId = context.Request.Headers.TryGetValue(
                        "X-Request-InternalCommandId",
                        out var requestIdHeader
                    )
                        ? requestIdHeader.FirstOrDefault()
                        : string.Empty;

                    return $"Management Service Apis, RequestId: {requestId}";
                }
            )
            .ExcludeFromDescription();

        return endpoints;
    }

    public WebApplicationBuilder AddSharedModuleServices(WebApplicationBuilder builder)
    {
        builder.AddInfrastructure();

        builder.AddStorage();

        return builder;
    }

    public async Task<WebApplication> ConfigureSharedModule(WebApplication app)
    {
        await app.UseInfrastructure();

        ServiceActivator.Configure(app.Services);

        await app.ApplyDatabaseMigrations();
        await app.SeedData();

        return app;
    }
}
