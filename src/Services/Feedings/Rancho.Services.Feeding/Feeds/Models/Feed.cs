using BuildingBlocks.Core.Domain;
using Rancho.Services.Feeding.Animals.Models;
using Rancho.Services.Feeding.Farms.Models;
using Rancho.Services.Feeding.FeedPlans.Models;
using Rancho.Services.Feeding.Feeds.ValueObjects;

namespace Rancho.Services.Feeding.Feeds.Models;

public class Feed : Aggregate<Guid>
{
    public Feed()
    {
        Id = Guid.NewGuid();
    }

    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Nutrition Nutrition { get; set; }
    public decimal WeightInStock { get; set; }

    public Guid FarmId { get; set; }
    public Farm Farm { get; set; }
    public ICollection<FeedPlan> FeedPlans { get; set; } = Enumerable.Empty<FeedPlan>().ToList();
}

