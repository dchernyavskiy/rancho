using BuildingBlocks.Core.Domain;
using Rancho.Services.Feeding.Animals.Enums;
using Rancho.Services.Feeding.FeedPlans.Models;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Farms.Models;

namespace Rancho.Services.Feeding.Animals.Models;

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
    public Farm Farm { get; set; }
    public ICollection<FeedPlan> FeedPlans { get; set; } = Enumerable.Empty<FeedPlan>().ToList();
}
