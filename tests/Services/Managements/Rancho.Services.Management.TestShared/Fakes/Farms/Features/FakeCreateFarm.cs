using Bogus;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1;
using Rancho.Services.Management.Farms.Features.CreatingFarm.v1;

namespace Rancho.Services.Management.TestShared.Fakes.Animals.Features;

public class FakeCreateFarm : Faker<CreateFarm>
{
    public FakeCreateFarm()
    {
        CustomInstantiator(
            f =>
            {
                return new CreateFarm(
                    f.Company.CompanyName(),
                    f.Address.FullAddress());
            });
    }
}
