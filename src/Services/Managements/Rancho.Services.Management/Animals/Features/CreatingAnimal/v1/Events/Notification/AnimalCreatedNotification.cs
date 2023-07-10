using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1.Events.Domain;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Management.Animals.Features.CreatingAnimal.v1.Events.Notification;

public record AnimalCreatedNotification(AnimalCreated DomainEvent)
    : BuildingBlocks.Core.CQRS.Events.Internal.DomainNotificationEventWrapper<AnimalCreated>(DomainEvent)
{
}

internal class AnimalCreatedNotificationHandler : IDomainNotificationEventHandler<AnimalCreatedNotification>
{
    private readonly IBus _bus;
    private readonly ILogger<AnimalCreatedNotificationHandler> _logger;

    public AnimalCreatedNotificationHandler(IBus bus, ILogger<AnimalCreatedNotificationHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(AnimalCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("In notification handler.");
        await _bus.PublishAsync(
            new AnimalCreatedV1(
                notification.DomainEvent.Animal.Id,
                notification.DomainEvent.Animal.Species,
                notification.DomainEvent.Animal.BirthDate,
                notification.DomainEvent.Animal.Gender.ToString(),
                notification.DomainEvent.Animal.EarTagNumber,
                notification.DomainEvent.Animal.FarmId),
            null,
            cancellationToken);
    }
}
