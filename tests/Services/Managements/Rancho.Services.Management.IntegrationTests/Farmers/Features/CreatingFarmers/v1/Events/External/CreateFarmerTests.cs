using FluentAssertions;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Animals.Features;
using Rancho.Services.Management.TestShared.Fakes.Farmers.Features;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Farmers.Features.CreatingFarmers.v1.Events.External;

public class CreateFarmerTests : ManagementServiceIntegrationTestBase
{
    private readonly Farm _farm;

    public CreateFarmerTests(
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
        var request = new FakeCreateFarmer(_farm.Id).Generate();

        var response = await SharedFixture.SendAsync(request);

        response.Should().NotBeNull();
        response.Farmer.FirstName.Should().Be(request.FirstName);
        response.Farmer.LastName.Should().Be(request.LastName);
        response.Farmer.Email.Should().Be(request.Email);
        response.Farmer.PhoneNumber.Should().Be(request.PhoneNumber);

        var farmer = await SharedFixture.FindEfDbContextAsync<Farmer>(response.Farmer.Id);

        farmer.Should().NotBeNull();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldPublishFarmerCreatedMessage()
    {
        var request = new FakeCreateFarmer(_farm.Id).Generate();

        var response = await SharedFixture.SendAsync(request);

        await SharedFixture.WaitForPublishing<FarmerCreatedV1>();
    }


    // [Fact]
    // [CategoryTrait(TestCategory.Integration)]
    // public async Task ShouldConsumeByExistingConsumerThroughTheBroker()
    // {
    //     await SharedFixture.PublishMessageAsync(_farmerCreated);
    //
    //     await SharedFixture.WaitForConsuming<FarmerCreatedV1>();
    // }
    //
    // [Fact]
    // [CategoryTrait(TestCategory.Integration)]
    // public async Task ShouldConsumeByFarmerCreatedConsumerThroughTheBroker()
    // {
    //     await SharedFixture.PublishMessageAsync(_farmerCreated);
    //
    //     await SharedFixture.WaitForConsuming<FarmerCreatedV1, FarmerCreatedConsumer>();
    // }
    //
    // [Fact]
    // [CategoryTrait(TestCategory.Integration)]
    // public async Task ShouldCreateNewFarmerInDbWhenAfterConsumingMessage()
    // {
    //     await SharedFixture.PublishMessageAsync(_farmerCreated);
    //
    //     await SharedFixture.WaitUntilConditionMet(
    //         async () =>
    //         {
    //             return await SharedFixture.ExecuteEfDbContextAsync(
    //                        async ctx =>
    //                        {
    //                            return await ctx.Farmers.AnyAsync(x => x.Id == _farmerCreated.Id);
    //                        });
    //         });
    // }
}
