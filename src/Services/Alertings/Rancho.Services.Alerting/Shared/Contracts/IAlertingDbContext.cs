using Microsoft.EntityFrameworkCore;
using Rancho.Services.Alerting.Alerts.Models;

namespace Rancho.Services.Alerting.Shared.Contracts;

public interface IAlertingDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<Alert> Alerts { get; }
}
