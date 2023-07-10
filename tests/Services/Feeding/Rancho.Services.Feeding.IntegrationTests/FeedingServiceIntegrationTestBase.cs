using BuildingBlocks.Security.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Rancho.Services.Feeding.Shared.Data;
using Rancho.Services.Feeding.TestShared.Fixtures;
using Tests.Shared.Fixtures;
using Tests.Shared.TestBase;
using Xunit.Abstractions;

namespace Rancho.Services.Feeding.IntegrationTests;

[Collection(IntegrationTestCollection.Name)]
public class FeedingServiceIntegrationTestBase
    : IntegrationTestBase<Api.Program, FeedingDbContext>
{
    public FeedingServiceIntegrationTestBase(
        SharedFixtureWithEfCore<Api.Program, FeedingDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    )
        : base(sharedFixture, outputHelper)
    {
        SharedFixture.Configuration["IdentityApiClientOptions:BaseApiAddress"] = FeedingServiceMockServersFixture
            .IdentityServiceMock
            .Url;
    }

    protected override void RegisterTestAppConfigurations(
        IConfigurationBuilder builder,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        base.RegisterTestAppConfigurations(builder, configuration, environment);
    }

    protected override void RegisterTestConfigureServices(IServiceCollection services)
    {
        services
            .Remove<ISecurityContextAccessor>()
            .AddSingleton<ISecurityContextAccessor>(
                config =>
                {
                    var mock = new Mock<ISecurityContextAccessor>();
                    mock.Setup(x => x.UserId).Returns(Guid.NewGuid().ToString());
                    mock.Setup(x => x.IsAuthenticated).Returns(true);
                    return mock.Object;
                });
    }

    public override Task DisposeAsync()
    {
        FeedingServiceMockServersFixture.IdentityServiceMock.Reset();

        return base.DisposeAsync();
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Remove<TService>(this IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(
            d =>
                d.ServiceType == typeof(TService));

        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }

        return services;
    }
}
