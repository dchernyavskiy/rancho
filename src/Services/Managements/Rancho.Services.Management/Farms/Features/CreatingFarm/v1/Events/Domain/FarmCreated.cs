
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Core.CQRS.Events.Internal;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Farms.Features.CreatingAnimal.v1.Events.Notification;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;

namespace Rancho.Services.Management.Farms.Features.CreatingAnimal.v1.Events.Domain;

public record FarmCreated(Farm Farm) : DomainEvent, IHaveNotificationEvent;

public class FarmCreatedDomainEventToIntegrationMappingHandler : IDomainEventHandler<FarmCreated>
{
    private readonly IDomainNotificationEventPublisher _publisher;
    private readonly ILogger<FarmCreatedDomainEventToIntegrationMappingHandler> _logger;
    private readonly IBus _bus;

    public FarmCreatedDomainEventToIntegrationMappingHandler(
        IDomainNotificationEventPublisher publisher,
        ILogger<FarmCreatedDomainEventToIntegrationMappingHandler> logger,
        IBus bus
    )
    {
        _publisher = publisher;
        _logger = logger;
        _bus = bus;
    }

    public async Task Handle(FarmCreated notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("In AnimalCreatedDomainEventToIntegrationMappingHandler");
        await _publisher.PublishAsync(new FarmCreatedNotification(notification), cancellationToken);
        // await _bus.PublishAsync(
        //     new FarmCreatedV1(notification.Farm.Id),
        //     null,
        //     cancellationToken);
    }
}
