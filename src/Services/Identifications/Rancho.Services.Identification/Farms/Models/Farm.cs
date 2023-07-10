using BuildingBlocks.Core.Domain;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Tags.Models;

namespace Rancho.Services.Identification.Farms.Models;

public class Farm : Aggregate<Guid>
{
    public Farm() : this(Guid.NewGuid()) { }

    public Farm(Guid id)
    {
        Id = id;
    }

    public Guid OwnerId { get; set; }

    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
    public ICollection<RfidTag> RfidTags { get; set; } = new List<RfidTag>();
}
