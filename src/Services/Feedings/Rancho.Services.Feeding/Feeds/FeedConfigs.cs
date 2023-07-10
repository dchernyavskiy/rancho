using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Rancho.Services.Feeding.Feeds.Data;
using Rancho.Services.Feeding.Shared;

namespace Rancho.Services.Feeding.Feeds;

public class FeedConfigs : IModuleConfiguration
{
    public const string Tag = "Feeds";
    public const string FeedsPrefixUri = $"{SharedModulesConfiguration.FeedingModulePrefixUri}/feeds";


    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        // builder.Services.AddScoped<IDataSeeder, FeedSeeder>();
        return builder;
    }

    public async Task<WebApplication> ConfigureModule(WebApplication app)
    {
        return app;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}
