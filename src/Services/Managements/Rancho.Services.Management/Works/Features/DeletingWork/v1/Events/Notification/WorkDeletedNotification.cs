// using BuildingBlocks.Abstractions.CQRS.Events.Internal;
// using BuildingBlocks.Abstractions.Messaging;
// using Microsoft.Extensions.Logging;
// using Rancho.Services.Management.Animals.Features.DeletingAnimal.v1.Events.Domain;
// using Rancho.Services.Management.Works.Features.DeletingWorks.v1.Events.Domain;
// using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
//
// namespace Rancho.Services.Management.Works.Features.DeletingWorks.v1.Events.Notification;
//
// public record WorkDeletedNotification(WorkDeleted DomainEvent)
//     : BuildingBlocks.Core.CQRS.Events.Internal.DomainNotificationEventWrapper<WorkDeleted>(DomainEvent);
//
// internal class AnimalDeletedNotificationHandler : IDomainNotificationEventHandler<WorkDeletedNotification>
// {
//     private readonly IBus _bus;
//     private readonly ILogger<AnimalDeletedNotificationHandler> _logger;
//
//     public AnimalDeletedNotificationHandler(IBus bus, ILogger<AnimalDeletedNotificationHandler> logger)
//     {
//         _bus = bus;
//         _logger = logger;
//     }
//
//     public async Task Handle(WorkDeletedNotification notification, CancellationToken cancellationToken)
//     {
//         _logger.LogInformation("In notification handler.");
//         await _bus.PublishAsync(
//             new AnimalDeletedV1(),
//             null,
//             cancellationToken);
//     }
// }
