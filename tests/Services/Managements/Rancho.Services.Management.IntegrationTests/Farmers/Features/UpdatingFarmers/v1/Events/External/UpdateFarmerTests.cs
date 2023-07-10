using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Farmers.Features.UpdatingFarmer.v1.Events.Integration.External;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Farmers.Events;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Farmers.Features.UpdatingFarmers.v1.Events.External;

public class UpdateFarmerTests : ManagementServiceIntegrationTestBase
{
    private readonly FarmerCreatedV1 _farmerCreated;
    private readonly FarmerUpdatedV1 _farmerUpdated;

    public UpdateFarmerTests(
        SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        _farmerCreated = new FakeFarmerCreatedV1().Generate();
        _farmerUpdated = new FarmerUpdatedV1(
            _farmerCreated.Id,
            _farmerCreated.FirstName + "1",
            _farmerCreated.LastName + "1");
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldConsumeByExistingConsumerThroughTheBroker()
    {
        await SharedFixture.PublishMessageAsync(_farmerCreated);
        await SharedFixture.WaitForConsuming<FarmerCreatedV1>();

        await SharedFixture.PublishMessageAsync(_farmerUpdated);
        await SharedFixture.WaitForConsuming<FarmerUpdatedV1>();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldConsumeByFarmerCreatedConsumerThroughTheBroker()
    {
        await SharedFixture.PublishMessageAsync(_farmerCreated);
        await SharedFixture.WaitForConsuming<FarmerCreatedV1>();

        await SharedFixture.PublishMessageAsync(_farmerUpdated);
        await SharedFixture.WaitForConsuming<FarmerUpdatedV1, FarmerUpdatedConsumer>();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldCreateNewFarmerInDbWhenAfterConsumingMessage()
    {
        await SharedFixture.PublishMessageAsync(_farmerCreated);
        await SharedFixture.WaitForConsuming<FarmerCreatedV1>();

        await SharedFixture.PublishMessageAsync(_farmerUpdated);

        await SharedFixture.WaitUntilConditionMet(
            async () =>
            {
                return await SharedFixture.ExecuteEfDbContextAsync(
                           async ctx =>
                           {
                               return !await ctx.Farmers.AnyAsync(
                                           x => x.Id == _farmerCreated.Id && x.FirstName == _farmerUpdated.FirstName);
                           });
            });
    }
}
