using Bogus;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

namespace Rancho.Services.Management.TestShared.Fakes.Farmers.Events;

public class FakeUserRegisteredV1 : Faker<UserRegisteredV1>
{
    public FakeUserRegisteredV1()
    {
        CustomInstantiator(
            f =>
            {
                return new UserRegisteredV1(
                    Guid.NewGuid(),
                    f.Person.Email,
                    f.Phone.PhoneNumber(),
                    f.Internet.UserName(),
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    new() {"farmer"});
            });
    }
}

public class FakeFarmerCreatedV1 : Faker<FarmerCreatedV1>
{
    public FakeFarmerCreatedV1()
    {
        CustomInstantiator(
            f =>
            {
                return new FarmerCreatedV1(
                    Guid.NewGuid(),
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    f.Internet.Email(),
                    f.Phone.PhoneNumber());
            });
    }
}
