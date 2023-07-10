using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Rancho.Services.Alerting.Alerts.Features.CreatingAlert.v1.Events.Integration.External;
using Rancho.Services.Shared.Taskings.Works.Events.v1.Integration;

namespace Rancho.Services.Alerting.Alerts;

internal static class MassTransitExtensions
{
    internal static void AddAlertEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint(
            nameof(WorkCreatedV1).Underscore().Prefixify(),
            re =>
            {
                re.ConfigureConsumeTopology = true;
                re.SetQuorumQueue();
                re.Bind(
                    $"{nameof(WorkCreatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(WorkCreatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Fanout;
                    });
                re.ConfigureConsumer<WorkCreatedConsumer>(context);

                re.RethrowFaultedMessages();
            });
    }
}
