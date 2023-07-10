using BuildingBlocks.Abstractions.Mapping;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Works.Dtos;
using Rancho.Services.Management.Works.Models;

namespace Rancho.Services.Management.Farmers.Dtos;

public class FarmerDto : IMapWith<Farmer>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public ICollection<WorkDto> Works { get; set; }
}
