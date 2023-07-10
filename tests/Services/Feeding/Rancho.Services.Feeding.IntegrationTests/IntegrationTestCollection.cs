using Rancho.Services.Feeding.Shared.Data;
using Rancho.Services.Feeding.TestShared.Fixtures;
using Tests.Shared.Fixtures;

namespace Rancho.Services.Feeding.IntegrationTests;

// https://stackoverflow.com/questions/43082094/use-multiple-collectionfixture-on-my-test-class-in-xunit-2-x
// note: each class could have only one collection, but it can implements multiple ICollectionFixture in its definitions
[CollectionDefinition(Name)]
public class IntegrationTestCollection
    : ICollectionFixture<
        SharedFixtureWithEfCore<
            Api.Program,
            FeedingDbContext
        >
    >,
        ICollectionFixture<FeedingServiceMockServersFixture>
{
    public const string Name = "Integration Test";
}
