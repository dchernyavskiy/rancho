using Bogus;
using MassTransit;
using Rancho.Services.Alerting.Alerts.Models;
using Rancho.Services.Alerting.Shared.Contracts;
using Rancho.Services.Shared.Taskings.Works.Events.v1.Integration;

namespace Rancho.Services.Alerting.Alerts.Features.CreatingAlert.v1.Events.Integration.External;

public class WorkCreatedConsumer : IConsumer<WorkCreatedV1>
{
    private readonly IAlertingDbContext _context;
    private readonly Faker _faker;

    public WorkCreatedConsumer(IAlertingDbContext context)
    {
        _context = context;
        _faker = new Faker();
    }

    public async Task Consume(ConsumeContext<WorkCreatedV1> context)
    {
        try
        {
            var entity = new Alert() {Name = _faker.Lorem.Word(), Description = _faker.Lorem.Letter()};

            await _context.Alerts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch
        {
        }
    }
}
