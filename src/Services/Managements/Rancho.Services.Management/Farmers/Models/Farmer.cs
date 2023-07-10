using BuildingBlocks.Core.Domain;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Works.Models;

namespace Rancho.Services.Management.Farmers.Models;

public class Farmer : Aggregate<Guid>
{
    public Farmer()
    {
        Id = Guid.NewGuid();
    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public Guid FarmId { get; set; }
    public Farm Farm { get; set; } = null!;
    public ICollection<Work> Works { get; set; }

    public static Farmer Create(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        Guid farmId
    )
    {
        return new Farmer
               {
                   Id = id,
                   FirstName = firstName,
                   LastName = lastName,
                   Email = email,
                   PhoneNumber = phoneNumber,
                   FarmId = farmId
               };
    }
}
