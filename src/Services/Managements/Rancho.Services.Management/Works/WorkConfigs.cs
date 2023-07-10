using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Rancho.Services.Management.Shared;
using Rancho.Services.Management.Works.Data;

namespace Rancho.Services.Management.Works;

public class WorkConfigs : IModuleConfiguration
{
    public const string Tag = "Works";
    public const string WorksPrefixUri = $"{SharedModulesConfiguration.ManagementModulePrefixUri}/works";

    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEventMapper, WorkEventMapper>();
        builder.Services.AddScoped<IDataSeeder, WorkDataSeeder>();
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
