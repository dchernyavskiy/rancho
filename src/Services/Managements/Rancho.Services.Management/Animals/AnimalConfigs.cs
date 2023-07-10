using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Rancho.Services.Management.Animals.Data;
using Rancho.Services.Management.Shared;

namespace Rancho.Services.Management.Animals;

public class AnimalConfigs : IModuleConfiguration
{
    public const string Tag = "Animals";
    public const string AnimalsPrefixUri = $"{SharedModulesConfiguration.ManagementModulePrefixUri}/animals";

    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEventMapper, AnimalEventMapper>();
        builder.Services.AddScoped<IDataSeeder, AnimalDataSeeder>();
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
