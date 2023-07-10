using FluentAssertions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using Rancho.Services.Management.Animals.Features.DeletingWork.v1;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Animals.Features;
using Rancho.Services.Management.TestShared.Fakes.Farmers.Events;
using Rancho.Services.Management.TestShared.Fakes.Works.Entities;
using Rancho.Services.Management.Works.Features.CreatingWork.v1;
using Rancho.Services.Management.Works.Features.UpdatingWork.v1;
using Rancho.Services.Management.Works.Models;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Works.Features.UpdatingWork.v1;

public class UpdateWorkTests : ManagementServiceIntegrationTestBase
{
    private readonly CreateWorkResponse _response;
    private readonly FarmerCreatedV1 _farmer;

    public UpdateWorkTests(
        SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        _farmer = new FakeFarmerCreatedV1().Generate();
        SharedFixture.PublishMessageAsync(_farmer).GetAwaiter().GetResult();
        var work = new FakeCreateWork(_farmer.Id).Generate();
        _response = sharedFixture.SendAsync(work).Result;
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldUpdateRecordInDb()
    {
        var request = new FakeUpdateWork(_farmer.Id, _response.Work.Id).Generate();

        await SharedFixture.SendAsync(request);
        var work = await SharedFixture.FindEfDbContextAsync<Work>(_response.Work.Id);

        work.Should().NotBeNull();
        work!.Name.Should().Be(request.Name);
        work!.Description.Should().Be(request.Description);
        work!.FarmerId.Should().Be(request.FarmerId);
    }
}
