using FluentAssertions;
using Rancho.Services.Management.Farms.Features.CreatingAnimal.v1.Events.Domain;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Animals.Features;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Farms.Features.CreatingFarm.v1;

public class CreateFarmTests : ManagementServiceIntegrationTestBase
{
    public CreateFarmTests(SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture, ITestOutputHelper outputHelper) : base(sharedFixture, outputHelper)
    {
    }


    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldCreateRecordInDb()
    {
        var request = new FakeCreateFarm().Generate();

        var response = await SharedFixture.SendAsync(request);

        response.Farm.Should().NotBeNull();
        response.Farm.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldPublishByExistingConsumerThroughTheBroker()
    {
        var request = new FakeCreateFarm().Generate();

        var response = await SharedFixture.SendAsync(request);

        await SharedFixture.WaitForPublishing<FarmCreatedV1>();
    }
}
