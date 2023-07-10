using FluentAssertions;
using Nito.AsyncEx;
using Rancho.Services.Management.Animals.Dtos;
using Rancho.Services.Management.Animals.Features.DeletingAnimal.v1;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Animals.Features;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Animals.Features.CreatingAnimal.v1;

public class DeleteAnimalTests : ManagementServiceIntegrationTestBase
{
    private readonly AnimalDto _animal;

    public DeleteAnimalTests(
        SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        var farm = sharedFixture.SendAsync(new FakeCreateFarm().Generate()).Result.Farm;
        _animal = sharedFixture.SendAsync(new FakeCreateAnimal(farm.Id).Generate()).Result.AnimalDto;
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldDeleteRecordFromDb()
    {
        var request = new DeleteAnimal(_animal.Id);

        await SharedFixture.SendAsync(request);
        var farm = await SharedFixture.FindEfDbContextAsync<Animal>(_animal.Id);

        farm.Should().BeNull();
    }
}
