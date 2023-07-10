using BuildingBlocks.Core.Domain;
using Rancho.Services.Feeding.Animals.Models;
using Rancho.Services.Feeding.Feeds.Models;

namespace Rancho.Services.Feeding.FeedPlans.Models;

public class FeedPlan : Aggregate<Guid>
{
    public FeedPlan()
    {
        Id = Guid.NewGuid();
    }

    public decimal WeightDispensed { get; set; }
    public decimal WeightEaten { get; set; }
    public DateTime DispenseDate { get; set; }
    public DateTime FixationDate { get; set; }

    public Guid AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;
    public Guid FeedId { get; set; }
    public Feed Feed { get; set; } = null!;
}
