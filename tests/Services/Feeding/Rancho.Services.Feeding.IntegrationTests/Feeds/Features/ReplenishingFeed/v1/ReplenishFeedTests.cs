using BuildingBlocks.Core.Exception.Types;
using FluentAssertions;
using Rancho.Services.Feeding.Feeds.Features.ReplenishingFeed.v1;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Shared.Data;
using Rancho.Services.Feeding.TestShared.Fakes.Feeds.Features;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Feeding.IntegrationTests.Feeds.Features.ReplenishingFeed.v1;

public class ReplenishFeedTests : FeedingServiceIntegrationTestBase
{
    private readonly Feed _feed;

    public ReplenishFeedTests(
        SharedFixtureWithEfCore<Feeding.Api.Program, FeedingDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        var request = new FakeRegisterFeed().Generate();
        var response = sharedFixture.SendAsync(request).Result;
        _feed = response.Feed;
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldChangeWeightInStock()
    {
        var weight = 10;
        var request = new ReplenishFeed(_feed.Id, weight);

        var response = await SharedFixture.SendAsync(request);

        response.Feed.WeightInStock.Should().Be(_feed.WeightInStock + weight);
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldThrowException()
    {
        var weight = -_feed.WeightInStock - 1;
        var request = new ReplenishFeed(_feed.Id, weight);

        var act = async () => await SharedFixture.SendAsync(request);

        await act.Should()
            .ThrowAsync<CustomException>()
            .WithMessage("The weight indicated is greater than what is in stock");
    }
}
