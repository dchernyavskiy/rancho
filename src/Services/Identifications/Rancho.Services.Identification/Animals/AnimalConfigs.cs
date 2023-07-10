using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Rancho.Services.Identification.Animals.Data;
using Rancho.Services.Identification.Shared;

namespace Rancho.Services.Identification.Animals;

public class AnimalConfigs : IModuleConfiguration
{
    public const string Tag = "Animals";
    public const string AnimalsPrefixUri = $"{SharedModulesConfiguration.IdentificationModulePrefixUri}/animals";

    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEventMapper, AnimalEventMapper>();
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
