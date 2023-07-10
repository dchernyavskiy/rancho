namespace Rancho.Services.Identification.Api.Extensions.ApplicationBuilderExtensions;

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
                    p.AllowAnyOrigin();
                    p.WithMethods("GET");
                    p.AllowAnyHeader();
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
