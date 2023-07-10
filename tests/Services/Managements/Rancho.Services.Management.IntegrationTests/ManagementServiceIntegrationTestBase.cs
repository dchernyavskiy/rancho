using BuildingBlocks.Core.Web.Extenions.ServiceCollection;
using BuildingBlocks.Security.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fixtures;
using Tests.Shared.Fixtures;
using Tests.Shared.TestBase;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests;

//https://stackoverflow.com/questions/43082094/use-multiple-collectionfixture-on-my-test-class-in-xunit-2-x
// note: each class could have only one collection
[Collection(IntegrationTestCollection.Name)]
public class ManagementServiceIntegrationTestBase
    : IntegrationTestBase<Api.Program, ManagementDbContext>
{
    // We don't need to inject `CustomersServiceMockServersFixture` class fixture in the constructor because it initialized by `collection fixture` and its static properties are accessible in the codes
    public ManagementServiceIntegrationTestBase(
        SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    )
        : base(sharedFixture, outputHelper)
    {
        // https://pcholko.com/posts/2021-04-05/wiremock-integration-test/
        // note1: for E2E test we use real identity service in on a TestContainer docker of this service, coordination with an external system is necessary in E2E

        // note2: add in-memory configuration instead of using appestings.json and override existing settings and it is accessible via IOptions and Configuration
        // https://blog.markvincze.com/overriding-configuration-in-asp-net-core-integration-tests/
        SharedFixture.Configuration["IdentityApiClientOptions:BaseApiAddress"] = ManagementServiceMockServersFixture
            .IdentityServiceMock
            .Url;
        // SharedFixture.Configuration["CatalogsApiClientOptions:BaseApiAddress"] = CustomersServiceMockServersFixture
        //     .CatalogsServiceMock
        //     .Url;

        // var catalogApiOptions = Scope.ServiceProvider.GetRequiredService<IOptions<CatalogsApiClientOptions>>();
        // var identityApiOptions = Scope.ServiceProvider.GetRequiredService<IOptions<IdentityApiClientOptions>>();
        //
        // identityApiOptions.Value.BaseApiAddress = MockServersFixture.IdentityServiceMock.Url!;
        // catalogApiOptions.Value.BaseApiAddress = MockServersFixture.CatalogsServiceMock.Url!;
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
        //// here we use same data seeder of service but if we need different data seeder for test for can replace it
        // services.ReplaceScoped<IDataSeeder, CustomersTestDataSeeder>();
        services.ReplaceSingleton<ISecurityContextAccessor>(
            provider =>
            {
                var mock = new Mock<ISecurityContextAccessor>();
                mock.Setup(x => x.UserId).Returns(Guid.NewGuid().ToString());
                mock.Setup(x => x.IsAuthenticated).Returns(true);
                return mock.Object;
            });
        var service = services.FirstOrDefault(d => d.ServiceType == typeof(ISecurityContextAccessor));
    }

    public override Task DisposeAsync()
    {
        // we should reset mappings routes we define in each test in end of running each test, but wiremock server is up in whole of test collection and is active for all tests
        // CustomersServiceMockServersFixture.CatalogsServiceMock.Reset();
        ManagementServiceMockServersFixture.IdentityServiceMock.Reset();

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
