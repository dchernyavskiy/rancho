using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Rancho.Services.Management.Animals.Data;
using Rancho.Services.Management.Farmers.Data;
using Rancho.Services.Management.Shared;

namespace Rancho.Services.Management.Farmers;

public class FarmerConfigs : IModuleConfiguration
{
    public const string Tag = "Farmers";
    public const string FarmersPrefixUri = $"{SharedModulesConfiguration.ManagementModulePrefixUri}/farmers";
    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        //builder.Services.AddSingleton<IEventMapper, FarmEventMapper>();
        builder.Services.AddScoped<IDataSeeder, FarmerDataSeeder>();
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
