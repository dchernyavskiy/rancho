using Bogus;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Feeding.TestShared.Fakes.Animals.Events;

public class FakeAnimalCreatedV1 : Faker<AnimalCreatedV1>
{
    public FakeAnimalCreatedV1()
    {
        CustomInstantiator(
            f =>
            {
                return new AnimalCreatedV1(
                    Guid.NewGuid(),
                    f.Random.String2(10),
                    f.Date.Past(1),
                    f.PickRandom("Male", "Female"),
                    f.Random.String2(10, 10),
                    Guid.NewGuid());
            });
    }
}
