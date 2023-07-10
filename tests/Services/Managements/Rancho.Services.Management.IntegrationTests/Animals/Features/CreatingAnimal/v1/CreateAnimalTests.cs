using FluentAssertions;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Animals.Features;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Animals.Features.CreatingAnimal.v1;

public class CreateAnimalTests : ManagementServiceIntegrationTestBase
{
    private readonly Farm _farm;

    public CreateAnimalTests(
        SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        var request = new FakeCreateFarm().Generate();
        _farm = SharedFixture.SendAsync(request).Result.Farm;
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldCreateRecordInDb()
    {
        var request = new FakeCreateAnimal(_farm.Id).Generate();

        var response = await SharedFixture.SendAsync(request);

        response.AnimalDto.Should().NotBeNull();
        response.AnimalDto.Id.Should().NotBe(Guid.Empty);

        var animal = await SharedFixture.FindEfDbContextAsync<Animal>(response.AnimalDto.Id);
        animal.Should().NotBeNull();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldThrowException()
    {
        var request = new FakeCreateAnimal(_farm.Id).Generate();

        var act = async () =>
                  {
                      await SharedFixture.SendAsync(request);
                      await SharedFixture.SendAsync(request);
                  };
        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldPublishAnimalCreatedMessage()
    {
        var request = new FakeCreateAnimal(_farm.Id).Generate();

        var response = await SharedFixture.SendAsync(request);

        await SharedFixture.WaitForPublishing<AnimalCreatedV1>();
    }
}
