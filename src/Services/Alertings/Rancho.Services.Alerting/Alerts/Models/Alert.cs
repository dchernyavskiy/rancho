using BuildingBlocks.Core.Domain;

namespace Rancho.Services.Alerting.Alerts.Models;

public class Alert : Aggregate<Guid>
{
    public Alert()
    {
        Id = Guid.NewGuid();
    }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
