using Rancho.Services.Management.TestShared.Fakes.Shared.Servers;
using Tests.Shared.Helpers;

namespace Rancho.Services.Management.TestShared.Fixtures;

public class ManagementServiceMockServersFixture : IAsyncLifetime
{
    public static IdentityServiceMock IdentityServiceMock { get; private set; } = default!;
    // public static CatalogsServiceMock CatalogsServiceMock { get; private set; } = default!;

    public Task InitializeAsync()
    {
        IdentityServiceMock = IdentityServiceMock.Start(ConfigurationHelper.BindOptions<IdentityApiClientOptions>());
        //CatalogsServiceMock = CatalogsServiceMock.Start(ConfigurationHelper.BindOptions<CatalogsApiClientOptions>());

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        IdentityServiceMock.Dispose();
        // CatalogsServiceMock.Dispose();

        return Task.CompletedTask;
    }
}

public class IdentityApiClientOptions
{
    public string BaseApiAddress { get; set; } = default!;
    public string UsersEndpoint { get; set; } = default!;
}
