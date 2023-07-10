using FluentAssertions;
using FluentResults;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Data;
using Rancho.Services.Management.TestShared.Fakes.Animals.Features;
using Rancho.Services.Management.TestShared.Fakes.Farmers.Events;
using Rancho.Services.Management.Works.Features.CreatingWork.v1;
using Rancho.Services.Management.Works.Models;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Management.IntegrationTests.Works.Features.CreatingWork.v1;

public class CreateWorkTests : ManagementServiceIntegrationTestBase
{
    private readonly FarmerCreatedV1 _farmer;

    public CreateWorkTests(SharedFixtureWithEfCore<Api.Program, ManagementDbContext> sharedFixture, ITestOutputHelper outputHelper) : base(sharedFixture, outputHelper)
    {
        _farmer = new FakeFarmerCreatedV1().Generate();
        SharedFixture.PublishMessageAsync(_farmer).GetAwaiter().GetResult();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldCreateRecordInDb()
    {
        var request = new CreateWork(
            "Name1",
            "Description",
            DateTime.Now,
            DateTime.Now.AddDays(10),
            _farmer.Id
            );

        var response = await SharedFixture.SendAsync(request);

        response.Work.Should().NotBeNull();
        response.Work.Id.Should().NotBe(Guid.Empty);

        var work = await SharedFixture.FindEfDbContextAsync<Work>(response.Work.Id);
        work.Should().NotBeNull();
    }
}
