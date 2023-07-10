using BuildingBlocks.Abstractions.CQRS.Commands;
using MediatR;

namespace Rancho.Services.Alerting.Alerts.Features.SendingAlert.v1;

public record SendAlert() : ICommand;

public class SendAlertHandler : ICommandHandler<SendAlert>
{
    public async Task<Unit> Handle(SendAlert request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}
