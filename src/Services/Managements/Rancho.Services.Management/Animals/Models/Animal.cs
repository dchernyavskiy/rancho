using BuildingBlocks.Core.Domain;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1.Events.Domain;
using Rancho.Services.Management.Animals.Features.DeletingAnimal.v1.Events.Domain;
using Rancho.Services.Management.Farms.Models;

namespace Rancho.Services.Management.Animals.Models;

public class Animal : Aggregate<Guid>
{
    public Animal()
    {
        Id = Guid.NewGuid();
    }

    public string Species { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string EarTagNumber { get; set; } = null!;
    public Guid FarmId { get; set; }
    public Farm Farm { get; set; } = null!;
}
