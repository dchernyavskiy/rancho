using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Rancho.Services.Management.Farms.Features.CreatingAnimal.v1.Events.Domain;
using Rancho.Services.Management.Farms.Features.CreatingAnimal.v1.Events.Notification;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;

namespace Rancho.Services.Management.Farms;

public class FarmEventMapper : IEventMapper
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
                   FarmCreated e => new FarmCreatedNotification(e), _ => null
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
                   FarmCreated e =>
                       new FarmCreatedV1(e.Farm.Id, e.Farm.Name, e.Farm.Address, e.Farm.OwnerId),
                   _ => null
               };
    }
}

public class FarmIntegrationEventMapper : IIntegrationEventMapper
{
    public IReadOnlyList<IIntegrationEvent?>? MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
    {
        return domainEvents.Select(MapToIntegrationEvent).ToList();
    }

    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
               {
                   FarmCreated e => new FarmCreatedV1(e.Farm.Id, e.Farm.Name, e.Farm.Address, e.Farm.OwnerId),
                   _ => null,
               };
    }
}
