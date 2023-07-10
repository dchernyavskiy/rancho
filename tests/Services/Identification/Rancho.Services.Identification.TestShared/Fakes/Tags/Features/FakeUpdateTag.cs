using Bogus;
using Rancho.Services.Identification.Tags.Enums;
using Rancho.Services.Identification.Tags.Features.UpdatingRfidTag.v1;

namespace Rancho.Services.Identification.TestShared.Fakes.Tags.Features;

public class FakeUpdateTag : Faker<UpdateRfidTag>
{
    public FakeUpdateTag(Guid animalId, Guid rfidTagId)
    {
        CustomInstantiator(
            f =>
            {
                return new UpdateRfidTag() {AnimalId = animalId, Id = rfidTagId, Status = f.PickRandom<Status>(),};
            });
    }
}
