using System.Reflection;
using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Alerting.Alerts.Models;
using Rancho.Services.Alerting.Shared.Contracts;

namespace Rancho.Services.Alerting.Shared.Data;

public class AlertingDbContext : EfDbContextBase, IAlertingDbContext
{
    public const string DefaultSchema = "alerting";

    public AlertingDbContext(DbContextOptions<AlertingDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Alert> Alerts => Set<Alert>();
}
