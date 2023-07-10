using FluentAssertions;
using Rancho.Services.Management.Animals.Dtos;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Animals.Features;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Animals.Features.CreatingAnimal.v1;

public class UpdateAnimalTests : ManagementServiceIntegrationTestBase
{
    private readonly AnimalDto _animal;

    public UpdateAnimalTests(
        SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        var farm = sharedFixture.SendAsync(new FakeCreateFarm().Generate()).Result.Farm;
        _animal = sharedFixture.SendAsync(new FakeCreateAnimal(farm.Id).Generate()).Result.AnimalDto;
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldUpdateRecordInDb()
    {
        var request = new FakeUpdateAnimal(_animal.Id).Generate();

        await SharedFixture.SendAsync(request);
        var animal = await SharedFixture.FindEfDbContextAsync<Animal>(request.Id);

        animal.Should().NotBeNull();
        animal!.Species.Should().Be(request.Species);
        animal.Gender.Should().Be(request.Gender);
    }
}
