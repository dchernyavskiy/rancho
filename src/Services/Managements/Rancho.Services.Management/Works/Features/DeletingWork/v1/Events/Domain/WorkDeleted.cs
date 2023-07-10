// using BuildingBlocks.Abstractions.CQRS.Events.Internal;
// using BuildingBlocks.Core.CQRS.Events.Internal;
// using Microsoft.Extensions.Logging;
// using Rancho.Services.Management.Animals.Features.DeletingAnimal.v1.Events.Notification;
// using Rancho.Services.Management.Works.Features.DeletingWorks.v1.Events.Notification;
//
// namespace Rancho.Services.Management.Works.Features.DeletingWorks.v1.Events.Domain;
//
// public record WorkDeleted() : DomainEvent;
//
// public class WorkDeletedDomainEventToIntegrationMappingHandler : IDomainEventHandler<WorkDeleted>
// {
//     private readonly IDomainNotificationEventPublisher _publisher;
//     private readonly ILogger<WorkDeletedDomainEventToIntegrationMappingHandler> _logger;
//
//     public WorkDeletedDomainEventToIntegrationMappingHandler(
//         IDomainNotificationEventPublisher publisher,
//         ILogger<WorkDeletedDomainEventToIntegrationMappingHandler> logger
//     )
//     {
//         _publisher = publisher;
//         _logger = logger;
//     }
//
//     public async Task Handle(WorkDeleted notification, CancellationToken cancellationToken)
//     {
//         await _publisher.PublishAsync(new WorkDeletedNotification(notification));
//     }
// }
