using Bogus;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Feeding.TestShared.Fakes.Animals.Events;

public class FakeAnimalUpdatedV1 : Faker<AnimalUpdatedV1>
{
    public FakeAnimalUpdatedV1(Guid animalId)
    {
        CustomInstantiator(
            f =>
            {
                return new AnimalUpdatedV1(
                    animalId,
                    f.Random.String2(10),
                    f.Date.Past(1),
                    f.PickRandom("Male", "Female"),
                    f.Random.String2(10, 10),
                    Guid.NewGuid());
            });
    }
}
