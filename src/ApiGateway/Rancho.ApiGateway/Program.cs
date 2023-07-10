using System.IdentityModel.Tokens.Jwt;
using BuildingBlocks.Logging;
using BuildingBlocks.Swagger;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SpectreConsole;
using Yarp.ReverseProxy.Transforms;

Log.Logger = new LoggerConfiguration().MinimumLevel
    .Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.SpectreConsole(
        "{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}",
        LogEventLevel.Information)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("yarp"))
    .AddTransforms(
        transforms =>
        {
            // https://microsoft.github.io/reverse-proxy/articles/transforms.html
            transforms.AddRequestTransform(
                transform =>
                {
                    var requestId = Guid.NewGuid().ToString("N");
                    var correlationId = Guid.NewGuid().ToString("N");

                    transform.ProxyRequest.Headers.Add("X-Request-InternalCommandId", requestId);
                    transform.ProxyRequest.Headers.Add("X-Correlation-InternalCommandId", correlationId);

                    return ValueTask.CompletedTask;
                });
        });


var app = builder.Build();

// request logging just log in information level and above as default

app.UseSerilogRequestLogging(
    opts =>
    {
        opts.EnrichDiagnosticContext = LogEnricher.EnrichFromRequest;
        opts.GetLevel = LogEnricher.GetLogLevel;
    });

app.MapGet(
    "/",
    async (HttpContext context) =>
    {
        await context.Response.WriteAsync($"Rancho Gateway");
    });
app.MapReverseProxy();

await app.RunAsync();
