using Serilog;
using Serilog.Core;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace Rancho.Services.Identity.Api.Extensions.ApplicationBuilderExtensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    ///     Register CORS.
    /// </summary>
    public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var clientAddress = configuration["ClientOptions:Address"];

        app.UseCors(
            p =>
            {
                if (string.IsNullOrEmpty(clientAddress))
                {
                    p.AllowAnyOrigin()
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    //     p.AllowAnyOrigin();
                    // p.WithMethods("GET");
                    // p.AllowAnyHeader();
                }
                else
                {
                    p.WithOrigins(clientAddress)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });

        return app;
    }
}
