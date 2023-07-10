using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fixtures;
using Tests.Shared.Fixtures;

namespace Rancho.Services.Management.IntegrationTests;

// https://stackoverflow.com/questions/43082094/use-multiple-collectionfixture-on-my-test-class-in-xunit-2-x
// note: each class could have only one collection, but it can implements multiple ICollectionFixture in its definitions
[CollectionDefinition(Name)]
public class IntegrationTestCollection
    : ICollectionFixture<
        SharedFixtureWithEfCore<
            Api.Program,
            ManagementDbContext
        >
    >,
        ICollectionFixture<ManagementServiceMockServersFixture>
{
    public const string Name = "Integration Test";
}
