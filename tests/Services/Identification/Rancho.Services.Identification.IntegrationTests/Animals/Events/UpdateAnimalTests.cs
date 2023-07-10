using Bogus.DataSets;
using FluentAssertions;
using FluentResults;
using Nito.AsyncEx;
using Rancho.Services.Identification.Animals.Enums;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Shared.Data;
using Rancho.Services.Identification.TestShared.Fakes.Animals.Events;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Identification.IntegrationTests.Animals.Events;

public class UpdateAnimalTests : IdentificationServiceIntegrationTestBase
{
    private readonly AnimalCreatedV1 _message;

    public UpdateAnimalTests(
        SharedFixtureWithEfCore<Api.Program, IdentificationDbContext> sharedFixture,
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
