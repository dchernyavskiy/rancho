using Rancho.Services.Feeding.FeedPlans.Features.UpdatingFeedPlan.v1;
using Rancho.Services.Feeding.Feeds.Features.RegisteringFeed.v1;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Shared.Data;
using Rancho.Services.Feeding.TestShared.Fakes.Animals.Events;
using Rancho.Services.Feeding.TestShared.Fakes.Feeds.Features;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Feeding.IntegrationTests.FeedPlans.Features.v1;

public class UpdateFeedPlanTests : FeedingServiceIntegrationTestBase
{
    private readonly AnimalCreatedV1 _animalCreated;
    private readonly Feed _feed;

    public UpdateFeedPlanTests(
        SharedFixtureWithEfCore<Api.Program, FeedingDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        _animalCreated = new FakeAnimalCreatedV1().Generate();
        sharedFixture.PublishMessageAsync(_animalCreated).GetAwaiter().GetResult();
        sharedFixture.WaitForConsuming<AnimalCreatedV1>().GetAwaiter().GetResult();

        var registerFeed = new FakeRegisterFeed().Generate();
        _feed = sharedFixture.SendAsync(registerFeed).Result.Feed;
    }


    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task UpdateFeedPlan_Should_Update_FeedPlan()
    {
        var request = new UpdateFeedPlan(_feed.Id, _animalCreated.Id, 5);


    }
}
