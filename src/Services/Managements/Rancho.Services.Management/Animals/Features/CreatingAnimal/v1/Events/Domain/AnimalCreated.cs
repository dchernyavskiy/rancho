using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1.Events.Notification;
using Rancho.Services.Management.Animals.Models;

namespace Rancho.Services.Management.Animals.Features.CreatingAnimal.v1.Events.Domain;

public record AnimalCreated(Animal Animal) : DomainEvent;

public class AnimalCreatedDomainEventToIntegrationMappingHandler : IDomainEventHandler<AnimalCreated>
{
    private readonly IDomainNotificationEventPublisher _publisher;
    private readonly ILogger<AnimalCreatedDomainEventToIntegrationMappingHandler> _logger;

    public AnimalCreatedDomainEventToIntegrationMappingHandler(IDomainNotificationEventPublisher publisher, ILogger<AnimalCreatedDomainEventToIntegrationMappingHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AnimalCreated notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("In AnimalCreatedDomainEventToIntegrationMappingHandler");
        await _publisher.PublishAsync(new AnimalCreatedNotification(notification), cancellationToken);
    }
}
