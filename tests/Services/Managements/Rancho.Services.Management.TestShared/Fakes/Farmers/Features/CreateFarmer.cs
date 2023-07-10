using Bogus;
using Rancho.Services.Management.Farmers.Features.CreatingFarmer.v1;
using Rancho.Services.Management.Farms.Models;

namespace Rancho.Services.Management.TestShared.Fakes.Farmers.Features;

public class FakeCreateFarmer : Faker<CreateFarmer>
{
    public FakeCreateFarmer(Guid farmId)
    {
        CustomInstantiator(
            f =>
            {
                return new CreateFarmer
                       {
                           FirstName = f.Name.FindName(),
                           LastName = f.Name.LastName(),
                           Email = f.Internet.Email(),
                           PhoneNumber = f.Phone.PhoneNumber(),
                           FarmId = farmId
                       };
            });
    }
}
