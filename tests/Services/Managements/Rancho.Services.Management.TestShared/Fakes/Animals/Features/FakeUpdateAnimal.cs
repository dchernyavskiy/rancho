using Bogus;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1;
using Rancho.Services.Management.Animals.Features.UpdatingAnimal.v1;

namespace Rancho.Services.Management.TestShared.Fakes.Animals.Features;

public class FakeUpdateAnimal : Faker<UpdateAnimal>
{
    public FakeUpdateAnimal(Guid animalId)
    {
        CustomInstantiator(
            f =>
            {
                return new UpdateAnimal
                       {
                           Id = animalId,
                           BirthDate = f.Date.Past(),
                           Gender = f.PickRandom<Gender>(),
                           Species = f.Lorem.Word()
                       };
            });
    }
}
