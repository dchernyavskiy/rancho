using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Farms.Features.CreatingAnimal.v1.Events.Domain;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;

namespace Rancho.Services.Management.Farms.Features.CreatingAnimal.v1.Events.Notification;

public record FarmCreatedNotification(FarmCreated DomainEvent)
    : BuildingBlocks.Core.CQRS.Events.Internal.DomainNotificationEventWrapper<FarmCreated>(DomainEvent);



internal class FarmCreatedNotificationHandler : IDomainNotificationEventHandler<FarmCreatedNotification>
{
    private readonly IBus _bus;
    private readonly ILogger<FarmCreatedNotificationHandler> _logger;

    public FarmCreatedNotificationHandler(
        IBus bus,
        ILogger<FarmCreatedNotificationHandler> logger
    )
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(FarmCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("In notification handler.");
        // await _bus.PublishAsync(
        //     new FarmCreatedV1(notification.DomainEvent.Farm.Id),
        //     null,
        //     cancellationToken);
    }
}
