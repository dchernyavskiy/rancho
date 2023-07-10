using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Animals.Features.DeletingAnimal.v1.Events.Notification;

namespace Rancho.Services.Management.Animals.Features.DeletingAnimal.v1.Events.Domain;

public record AnimalDeleted(Guid Id) : DomainEvent;

public class AnimalDeletedDomainEventToIntegrationMappingHandler : IDomainEventHandler<AnimalDeleted>
{
    private readonly IDomainNotificationEventPublisher _publisher;
    private readonly ILogger<AnimalDeletedDomainEventToIntegrationMappingHandler> _logger;

    public AnimalDeletedDomainEventToIntegrationMappingHandler(
        IDomainNotificationEventPublisher publisher,
        ILogger<AnimalDeletedDomainEventToIntegrationMappingHandler> logger
    )
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AnimalDeleted notification, CancellationToken cancellationToken)
    {
        await _publisher.PublishAsync(new AnimalDeletedNotification(notification));
    }
}
