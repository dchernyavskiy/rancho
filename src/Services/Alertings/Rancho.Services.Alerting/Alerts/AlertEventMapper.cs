using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;

namespace Rancho.Services.Alerting.Alerts;

public class AlertEventMapper : IEventMapper
{
    public IReadOnlyList<IDomainNotificationEvent?>? MapToDomainNotificationEvents(
        IReadOnlyList<IDomainEvent> domainEvents
    )
    {
        return domainEvents.Select(MapToDomainNotificationEvent).ToList();
    }

    public IDomainNotificationEvent? MapToDomainNotificationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
               {
                   _ => null
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
                   _ => null
               };
    }
}
