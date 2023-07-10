using FluentAssertions;
using Rancho.Services.Management.Farms.Features.DeletingFarm.v1;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Animals.Features;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Farms.Features.CreatingFarm.v1;

public class DeleteFarmTests : ManagementServiceIntegrationTestBase
{
    private readonly Farm _farm;

    public DeleteFarmTests(
        SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        var request = new FakeCreateFarm().Generate();
        _farm = SharedFixture.SendAsync(request).Result.Farm;
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldDeleteRecordFromDb()
    {
        var request = new DeleteFarm(_farm.Id);

        await SharedFixture.SendAsync(request);
        var farm = await SharedFixture.FindEfDbContextAsync<Farm>(_farm.Id);

        farm.Should().BeNull();
    }
}
