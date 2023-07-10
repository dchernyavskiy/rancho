using BuildingBlocks.Core.Domain;
using Rancho.Services.Feeding.Animals.Models;
using Rancho.Services.Feeding.Feeds.Models;

namespace Rancho.Services.Feeding.Farms.Models;

public class Farm : Aggregate<Guid>
{
    public Farm() : this(Guid.NewGuid()) { }

    public Farm(Guid id)
    {
        Id = id;
    }

    public Guid OwnerId { get; set; }

    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
    public ICollection<Feed> Feeds { get; set; } = new List<Feed>();
}
