using FluentAssertions;
using Rancho.Services.Management.Animals.Features.GettingListOfAnimals.v1;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Animals.Features;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Animals.Features.CreatingAnimal.v1;

public class GetListOfAnimalTests : ManagementServiceIntegrationTestBase
{
    private readonly Farm _farm;

    public GetListOfAnimalTests(SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture, ITestOutputHelper outputHelper) : base(sharedFixture, outputHelper)
    {
        var request = new FakeCreateFarm().Generate();
        _farm = SharedFixture.SendAsync(request).Result.Farm;
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldReturnEmptyCollection()
    {
        var request = new GetListOfAnimals();

        var response = await SharedFixture.SendAsync(request);

        response.Body.Items.Should().BeEmpty();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldReturnNotEmptyCollection()
    {
        for (int i = 0; i < 10; i++)
        {
            await SharedFixture.SendAsync(new FakeCreateAnimal(_farm.Id).Generate());
        }

        var request = new GetListOfAnimals();

        var response = await SharedFixture.SendAsync(request);

        response.Body.Items.Should().NotBeEmpty();
    }
}
