using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Farmers.Features.CreatingFarmer.v1.Events.Integration.External;
using Rancho.Services.Management.Farmers.Features.DeletingFarmer.v1.Events.Integration.External;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Farmers.Events;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Farmers.Features.DeletingFarmers.v1.Events.External;

public class DeleteFarmerTests : ManagementServiceIntegrationTestBase
{
    private readonly FarmerCreatedV1 _farmerCreated;
    private readonly FarmerDeletedV1 _farmerDeleted;

    public DeleteFarmerTests(
        SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        _farmerCreated = new FakeFarmerCreatedV1().Generate();
        _farmerDeleted = new FarmerDeletedV1(_farmerCreated.Id);
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldConsumeByExistingConsumerThroughTheBroker()
    {
        await SharedFixture.PublishMessageAsync(_farmerCreated);
        await SharedFixture.WaitForConsuming<FarmerCreatedV1>();

        await SharedFixture.PublishMessageAsync(_farmerDeleted);
        await SharedFixture.WaitForConsuming<FarmerDeletedV1>();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldConsumeByFarmerCreatedConsumerThroughTheBroker()
    {
        await SharedFixture.PublishMessageAsync(_farmerCreated);
        await SharedFixture.WaitForConsuming<FarmerCreatedV1>();

        await SharedFixture.PublishMessageAsync(_farmerDeleted);
        await SharedFixture.WaitForConsuming<FarmerDeletedV1, FarmerDeletedConsumer>();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldCreateNewFarmerInDbWhenAfterConsumingMessage()
    {
        await SharedFixture.PublishMessageAsync(_farmerCreated);
        await SharedFixture.WaitForConsuming<FarmerCreatedV1>();

        await SharedFixture.PublishMessageAsync(_farmerDeleted);

        await SharedFixture.WaitUntilConditionMet(
            async () =>
            {
                return await SharedFixture.ExecuteEfDbContextAsync(
                           async ctx =>
                           {
                               return !await ctx.Farmers.AnyAsync(x => x.Id == _farmerCreated.Id);
                           });
            });
    }
}
