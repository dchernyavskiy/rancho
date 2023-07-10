using FluentAssertions;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Shared.Data;
using Rancho.Services.Identification.TestShared.Fakes.Animals.Events;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Identification.IntegrationTests.Animals.Events;

public class CreateAnimalTests : IdentificationServiceIntegrationTestBase
{
    public CreateAnimalTests(SharedFixtureWithEfCore<Api.Program, IdentificationDbContext> sharedFixture, ITestOutputHelper outputHelper) : base(sharedFixture, outputHelper)
    {
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldConsumeByExistingConsumerThroughTheBroker()
    {
        var message = new FakeAnimalCreatedV1().Generate();

        await SharedFixture.PublishMessageAsync(message);

        await SharedFixture.WaitForConsuming<AnimalCreatedV1>();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldCreateRecordInDb()
    {
        var message = new FakeAnimalCreatedV1().Generate();

        await SharedFixture.PublishMessageAsync(message);
        await SharedFixture.WaitForConsuming<AnimalCreatedV1>();

        var animal = SharedFixture.FindEfDbContextAsync<Animal>(message.Id);
        animal.Should().NotBeNull();
    }
}
