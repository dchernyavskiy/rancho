using BuildingBlocks.Core.Domain;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1.Events.Domain;
using Rancho.Services.Management.Animals.Features.DeletingAnimal.v1.Events.Domain;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Works.Enums;

namespace Rancho.Services.Management.Works.Models;

public class Work : Aggregate<Guid>
{
    public Work()
    {
        Id = Guid.NewGuid();
    }

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public WorkStatus Status { get; set; }

    public Guid FarmerId { get; set; }
    public Farmer Farmer { get; set; } = null!;
}
