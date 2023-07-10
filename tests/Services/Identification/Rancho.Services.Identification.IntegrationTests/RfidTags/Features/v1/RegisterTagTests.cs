using FluentAssertions;
using Rancho.Services.Identification.Shared.Data;
using Rancho.Services.Identification.Tags.Features.RegisteringRfidTag.v1;
using Rancho.Services.Identification.Tags.Models;
using Rancho.Services.Identification.TestShared.Fakes.Animals.Events;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Identification.IntegrationTests.RfidTags.Features.v1;

public class RegisterTagTests : IdentificationServiceIntegrationTestBase
{
    public RegisterTagTests(
        SharedFixtureWithEfCore<Api.Program, IdentificationDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldCreateRecordInDb()
    {
        var request = new RegisterRfidTag("000 9889 4343 3434");

        var response = await SharedFixture.SendAsync(request);

        response.Should().NotBeNull();
        response.Tag.Should().NotBeNull();
        response.Tag.Code.Should().BeEquivalentTo(request.Code);
        response.Tag.Id.Should().NotBe(Guid.Empty);

        var record = await SharedFixture.FindEfDbContextAsync<RfidTag>(response.Tag.Id);
        record.Should().NotBeNull();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task RegisterWithAnimalEarTagNumber_ShouldCreateRecordInDb()
    {
        var message = new FakeAnimalCreatedV1().Generate();
        await SharedFixture.PublishMessageAsync(message);
        await SharedFixture.WaitForConsuming<AnimalCreatedV1>();
        var request = new RegisterRfidTag("000 9889 4343 3434") {EarTagNumber = message.EarTagNumber};

        var response = await SharedFixture.SendAsync(request);

        response.Should().NotBeNull();
        response.Tag.Should().NotBeNull();
        response.Tag.Code.Should().BeEquivalentTo(request.Code);
        response.Tag.Id.Should().NotBe(Guid.Empty);

        var record = await SharedFixture.FindEfDbContextAsync<RfidTag>(response.Tag.Id);
        record.Should().NotBeNull();
    }
}
