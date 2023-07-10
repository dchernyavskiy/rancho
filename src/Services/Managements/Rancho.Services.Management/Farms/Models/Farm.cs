using BuildingBlocks.Core.Domain;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Farms.Features.CreatingAnimal.v1.Events.Domain;

namespace Rancho.Services.Management.Farms.Models;

public class Farm : Aggregate<Guid>
{
    public Farm()
    {
        Id = Guid.NewGuid();
    }

    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public Guid OwnerId { get; set; }

    public ICollection<Farmer> Farmers { get; set; } = new List<Farmer>();
    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
