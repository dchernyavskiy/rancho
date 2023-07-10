using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Animals.Features.CreatingWork.v1.Events.Notification;
using Rancho.Services.Management.Animals.Models;

namespace Rancho.Services.Management.Works.Features.CreatingWork.v1.Events.Domain;

public record WorkCreated(Animal Animal) : DomainEvent;

public class WorkCreatedDomainEventToIntegrationMappingHandler : IDomainEventHandler<WorkCreated>
{
    private readonly IDomainNotificationEventPublisher _publisher;
    private readonly ILogger<WorkCreatedDomainEventToIntegrationMappingHandler> _logger;

    public WorkCreatedDomainEventToIntegrationMappingHandler(IDomainNotificationEventPublisher publisher, ILogger<WorkCreatedDomainEventToIntegrationMappingHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(WorkCreated notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("In AnimalCreatedDomainEventToIntegrationMappingHandler");
        await _publisher.PublishAsync(new WorkCreatedNotification(notification), cancellationToken);
    }
}
