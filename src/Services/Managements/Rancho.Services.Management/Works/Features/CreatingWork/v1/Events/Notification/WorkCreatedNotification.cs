using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Works.Features.CreatingWork.v1.Events.Domain;
using Rancho.Services.Shared.Taskings.Works.Events.v1.Integration;

namespace Rancho.Services.Management.Animals.Features.CreatingWork.v1.Events.Notification;

public record WorkCreatedNotification(WorkCreated DomainEvent)
    : BuildingBlocks.Core.CQRS.Events.Internal.DomainNotificationEventWrapper<WorkCreated>(DomainEvent)
{
}

internal class WorkCreatedNotificationHandler : IDomainNotificationEventHandler<WorkCreatedNotification>
{
    private readonly IBus _bus;
    private readonly ILogger<WorkCreatedNotificationHandler> _logger;

    public WorkCreatedNotificationHandler(IBus bus, ILogger<WorkCreatedNotificationHandler> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task Handle(WorkCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("In notification handler.");
        await _bus.PublishAsync(
            new WorkCreatedV1(),
            null,
            cancellationToken);
    }
}
