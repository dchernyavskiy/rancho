using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Logging;
using Rancho.Services.Feeding.Farms.Models;
using Rancho.Services.Feeding.Shared.Contracts;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;

namespace Rancho.Services.Feeding.Farms.Features.CreatingFarm.v1.Events.Integration.External;

public class FarmCreatedConsumer : IConsumer<FarmCreatedV1>
{
    private readonly IFeedingDbContext _context;
    private readonly ILogger<FarmCreatedConsumer> _logger;

    public FarmCreatedConsumer(IFeedingDbContext context, ILogger<FarmCreatedConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<FarmCreatedV1> context)
    {
        try
        {
            _logger.LogInformation("In Farm Created Consumer.");
            var farm = new Farm(context.Message.Id) {OwnerId = context.Message.OwnerId};

            await _context.Farms.AddAsync(farm);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                $"{nameof(FarmCreatedV1)} was consumed by {Assembly.GetExecutingAssembly().FullName}.");
            _logger.LogInformation($"Farm with id {farm.Id} was created");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
