using FluentAssertions;
using Rancho.Services.Feeding.Animals.Models;
using Rancho.Services.Feeding.Shared.Data;
using Rancho.Services.Feeding.TestShared.Fakes.Animals.Events;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Feeding.IntegrationTests.IntegrationTests.Animals.Events;

public class DeleteAnimalTests : FeedingServiceIntegrationTestBase
{
    private readonly AnimalCreatedV1 _message;

    public DeleteAnimalTests(
        SharedFixtureWithEfCore<Api.Program, FeedingDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        _message = new FakeAnimalCreatedV1().Generate();
        sharedFixture.PublishMessageAsync(_message).GetAwaiter().GetResult();
        SharedFixture.WaitForConsuming<AnimalCreatedV1>().GetAwaiter().GetResult();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldConsumeByExistingConsumerThroughTheBroker()
    {
        var message = new AnimalDeletedV1(_message.Id);

        await SharedFixture.PublishMessageAsync(message);

        await SharedFixture.WaitForConsuming<AnimalDeletedV1>();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldDeleteRecordFromDb()
    {
        var message = new AnimalDeletedV1(_message.Id);

        await SharedFixture.PublishMessageAsync(message);
        await SharedFixture.WaitForConsuming<AnimalDeletedV1>();
        await Task.Delay(3000);

        var animal = await SharedFixture.FindEfDbContextAsync<Animal>(message.Id);
        animal.Should().BeNull();
    }
}
