using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Rancho.Services.Management.Animals.Data;
using Rancho.Services.Management.Shared;

namespace Rancho.Services.Management.Farms;

public class FarmConfigs : IModuleConfiguration
{
    public const string Tag = "Farms";
    public const string FarmsPrefixUri = $"{SharedModulesConfiguration.ManagementModulePrefixUri}/farms";
    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEventMapper, FarmEventMapper>();
        builder.Services.AddSingleton<IIntegrationEventMapper, FarmIntegrationEventMapper>();
        builder.Services.AddScoped<IDataSeeder, FarmDataSeeder>();
        return builder;
    }

    public Task<WebApplication> ConfigureModule(WebApplication app)
    {
        return Task.FromResult(app);
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }
}
