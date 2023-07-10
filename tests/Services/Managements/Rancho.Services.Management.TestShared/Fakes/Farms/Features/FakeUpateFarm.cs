using Bogus;
using Rancho.Services.Management.Farms.Features.UpdatingFarm.v1;

namespace Rancho.Services.Management.TestShared.Fakes.Animals.Features;

public class FakeUpdateFarm : Faker<UpdateFarm>
{
    public FakeUpdateFarm(Guid farmId)
    {
        CustomInstantiator(
            f =>
            {
                return new UpdateFarm{
                    Id = farmId,
                    Name = f.Company.CompanyName(),
                    Address = f.Address.FullAddress()
                    };
            });
    }
}
