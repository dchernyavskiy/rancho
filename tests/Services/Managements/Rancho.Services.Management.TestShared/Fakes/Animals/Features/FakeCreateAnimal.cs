using Bogus;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1;

namespace Rancho.Services.Management.TestShared.Fakes.Animals.Features;

public class FakeCreateAnimal : Faker<CreateAnimal>
{
    public FakeCreateAnimal(Guid farmId)
    {
        CustomInstantiator(
            f =>
            {
                return new CreateAnimal(
                    f.Lorem.Word(),
                    f.Date.Past(),
                    f.PickRandom<Gender>(),
                    f.Lorem.Word(),
                    farmId);
            });
    }
}
