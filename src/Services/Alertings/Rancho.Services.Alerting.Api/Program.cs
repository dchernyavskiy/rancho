using Bogus;
using BuildingBlocks.Core.Web;
using BuildingBlocks.Core.Web.Extenions;
using BuildingBlocks.Core.Web.Extenions.ServiceCollection;
using BuildingBlocks.Swagger;
using BuildingBlocks.Web;
using BuildingBlocks.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Rancho.Services.Alerting.Api.Extensions.ApplicationBuilderExtensions;
using Spectre.Console;

AnsiConsole.Write(new FigletText("Alerting Service").Centered().Color(Color.FromInt32(new Faker().Random.Int(1, 255))));

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(
    (context, options) =>
    {
        options.ValidateScopes =
            context.HostingEnvironment.IsDevelopment()
            || context.HostingEnvironment.IsTest()
            || context.HostingEnvironment.IsStaging();
    }
);

builder.Services
    .AddControllers(
        options => options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()))
    )
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddValidatedOptions<AppOptions>();
builder.AddMinimalEndpoints();

builder.AddModulesServices();

var app = builder.Build();

await app.ConfigureModules();

app.UseAppCors();
app.MapModulesEndpoints();
app.MapMinimalEndpoints();
app.MapControllers();

//if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker"))
//{
    app.UseCustomSwagger();
//}

await app.RunAsync();

public partial class Program { }
