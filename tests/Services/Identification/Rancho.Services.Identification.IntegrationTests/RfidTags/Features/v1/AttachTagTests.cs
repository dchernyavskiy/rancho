using FluentAssertions;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Shared.Data;
using Rancho.Services.Identification.Tags.Features.AttachingRfidTag.v1;
using Rancho.Services.Identification.Tags.Features.RegisteringRfidTag.v1;
using Rancho.Services.Identification.Tags.Models;
using Rancho.Services.Identification.TestShared.Fakes.Animals.Events;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Identification.IntegrationTests.RfidTags.Features.v1;

public class AttachTagTests : IdentificationServiceIntegrationTestBase
{
    private readonly AnimalCreatedV1 _message;
    private readonly RfidTag _tag;

    public AttachTagTests(
        SharedFixtureWithEfCore<Api.Program, IdentificationDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        _message = new FakeAnimalCreatedV1().Generate();
        sharedFixture.PublishMessageAsync(_message).GetAwaiter().GetResult();
        SharedFixture.WaitForConsuming<AnimalCreatedV1>().GetAwaiter().GetResult();

        var request = new RegisterRfidTag("000 9889 4343 3434");
        _tag = SharedFixture.SendAsync(request).Result.Tag;
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldAttachTagToAnimalRecordInDb()
    {
        var request = new AttachRfidTag() {AnimalId = _message.Id, RfidTagId = _tag.Id};

        await SharedFixture.SendAsync(request);

        var animal = await SharedFixture.FindEfDbContextAsync<Animal>(_message.Id);
        animal.Should().NotBeNull();
        animal!.RfidTagId.Should().Be(_tag.Id);

        var tag = await SharedFixture.FindEfDbContextAsync<RfidTag>(_tag.Id);
        tag.Should().NotBeNull();
        tag!.AnimalId.Should().Be(_message.Id);
    }
}
