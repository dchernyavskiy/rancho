using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1.Events.Domain;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1.Events.Notification;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Management.Animals;

public class AnimalEventMapper : IEventMapper
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
                   AnimalCreated e => new AnimalCreatedNotification(e), _ => null
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
                   AnimalCreated e =>
                       new AnimalCreatedV1(
                           e.Animal.Id,
                           e.Animal.Species,
                           e.Animal.BirthDate,
                           e.Animal.Gender.ToString(),
                           e.Animal.EarTagNumber,
                           e.Animal.FarmId),
                   _ => null
               };
    }
}
