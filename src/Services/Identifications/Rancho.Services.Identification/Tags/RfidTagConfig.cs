using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Rancho.Services.Identification.Shared;
using Rancho.Services.Identification.Tags.Data;

namespace Rancho.Services.Identification.Tags;

public class RfidTagConfig : IModuleConfiguration
{
    public const string Tag = "Tags";
    public const string TagsPrefixUri = $"{SharedModulesConfiguration.IdentificationModulePrefixUri}/rfidtags";

    public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder)
    {
        // builder.Services.AddScoped<IDataSeeder, RfidTagDataSeeder>();
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
