using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Rancho.Services.Management.Animals.Features.CreatingWork.v1.Events.Notification;
using Rancho.Services.Management.Works.Features.CreatingWork.v1.Events.Domain;
using Rancho.Services.Shared.Taskings.Works.Events.v1.Integration;

namespace Rancho.Services.Management.Works;

public class WorkEventMapper : IEventMapper
{
    public IReadOnlyList<IDomainNotificationEvent?>? MapToDomainNotificationEvents(
        IReadOnlyList<IDomainEvent> domainEvents
    )
    {
        return domainEvents.Select(MapToDomainNotificationEvent).ToList().AsReadOnly();
    }

    public IDomainNotificationEvent? MapToDomainNotificationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
               {
                   WorkCreated e => new WorkCreatedNotification(e), _ => null
               };
    }

    public IReadOnlyList<IIntegrationEvent?>? MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
    {
        return domainEvents.Select(MapToIntegrationEvent).ToList().AsReadOnly();
    }

    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
               {
                   WorkCreated e =>
                       new WorkCreatedV1(),
                   _ => null
               };
    }
}
