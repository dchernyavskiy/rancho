using MassTransit;
using Microsoft.Extensions.Logging;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;

namespace Rancho.Services.Alerting.Farms.Features.CreatingFarm.v1.Events.Integration.External;

public class FarmCreatedConsumer : IConsumer<FarmCreatedV1>
{
    private readonly ILogger<FarmCreatedConsumer> _logger;

    public FarmCreatedConsumer(ILogger<FarmCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<FarmCreatedV1> context)
    {
        _logger.LogInformation($"Farm created: {context.Message.Id}");
    }
}
