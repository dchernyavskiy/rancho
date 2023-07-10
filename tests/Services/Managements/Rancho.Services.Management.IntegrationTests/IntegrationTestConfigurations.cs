using Tests.Shared;
using Tests.Shared.Fixtures;

namespace Rancho.Services.Management.IntegrationTests;

public class IntegrationTestConfigurations : TestConfigurations
{
    public IntegrationTestConfigurations()
    {
        this["ASPNETCORE_ENVIRONMENT"] = "test";
    }
}
