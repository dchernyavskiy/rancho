using FluentAssertions;
using Rancho.Services.Feeding.Feeds.Features.GettingFeeds.v1;
using Rancho.Services.Feeding.Shared.Data;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Feeding.IntegrationTests.Feeds.Features.GettingFeeds.v1;

public class GetFeedsTests : FeedingServiceIntegrationTestBase
{
    public GetFeedsTests(SharedFixtureWithEfCore<Feeding.Api.Program, FeedingDbContext> sharedFixture, ITestOutputHelper outputHelper) : base(sharedFixture, outputHelper)
    {
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldGetEmptyCollection()
    {
        var request = new GetFeeds();

        var response = await SharedFixture.SendAsync(request);

        response.Feeds.Should().BeEmpty();
    }
}
