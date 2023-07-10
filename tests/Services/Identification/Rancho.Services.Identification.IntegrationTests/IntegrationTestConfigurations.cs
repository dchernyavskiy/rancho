using Tests.Shared;
using Tests.Shared.Fixtures;

namespace Rancho.Services.Identification.IntegrationTests;

public class IntegrationTestConfigurations : TestConfigurations
{
    public IntegrationTestConfigurations()
    {
        this["ASPNETCORE_ENVIRONMENT"] = "test";
    }
}
