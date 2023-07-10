using FluentAssertions;
using Rancho.Services.Feeding.Animals.Enums;
using Rancho.Services.Feeding.Animals.Models;
using Rancho.Services.Feeding.Shared.Data;
using Rancho.Services.Feeding.TestShared.Fakes.Animals.Events;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Feeding.IntegrationTests.IntegrationTests.Animals.Events;

public class UpdateAnimalTests : FeedingServiceIntegrationTestBase
{
    private readonly AnimalCreatedV1 _message;

    public UpdateAnimalTests(
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
        var message = new FakeAnimalUpdatedV1(_message.Id).Generate();

        await SharedFixture.PublishMessageAsync(message);

        await SharedFixture.WaitForConsuming<AnimalUpdatedV1>();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldUpdateRecordInDb()
    {
        var message = new FakeAnimalUpdatedV1(_message.Id).Generate();

        await SharedFixture.PublishMessageAsync(message);
        await SharedFixture.WaitForConsuming<AnimalUpdatedV1>();

        var animal = await SharedFixture.FindEfDbContextAsync<Animal>(message.Id);
        animal.Should().NotBeNull();
        animal!.Species.Should().BeEquivalentTo(message.Species);
        animal.Gender.Should().Be(Enum.Parse<Gender>(message.Gender));
        animal.EarTagNumber.Should().BeEquivalentTo(message.EarTagNumber);
        animal.FarmId.Should().Be(message.FarmId);
    }
}
