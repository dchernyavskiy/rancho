using BuildingBlocks.Core.Domain;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Farms.Models;
using Rancho.Services.Identification.Tags.Enums;

namespace Rancho.Services.Identification.Tags.Models;

public class RfidTag : Aggregate<Guid>
{
    public RfidTag()
    {
        Id = Guid.NewGuid();
    }

    public Status Status { get; set; }
    public Guid? AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;
    public Guid? FarmId { get; set; }
    public Farm Farm { get; set; } = null!;
}
