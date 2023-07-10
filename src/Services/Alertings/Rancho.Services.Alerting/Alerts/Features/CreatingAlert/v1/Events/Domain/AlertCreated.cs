using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using MediatR;
using Rancho.Services.Alerting.Alerts.Models;

namespace Rancho.Services.Alerting.Alerts.Features.CreatingAlert.v1.Events.Domain;

public record AlertCreated(Alert Alert) : DomainEvent;

public class AlertCreatedHandler : IDomainEventHandler<AlertCreated>
{
    private readonly ISender _sender;


    public AlertCreatedHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task Handle(AlertCreated notification, CancellationToken cancellationToken)
    {
    }
}
