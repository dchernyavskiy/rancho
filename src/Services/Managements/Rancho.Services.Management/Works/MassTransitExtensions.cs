using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Rancho.Services.Shared.Taskings.Works.Events.v1.Integration;

namespace Rancho.Services.Management.Works;

public static class MassTransitExtensions
{
    internal static void AddAnimalPublishers(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Message<WorkCreatedV1>(e => e.SetEntityName($"{nameof(WorkCreatedV1).Underscore()}.input_exchange"));
        cfg.Publish<WorkCreatedV1>(e => e.ExchangeType = ExchangeType.Fanout);
        cfg.Send<WorkCreatedV1>(
            e =>
            {
                e.UseRoutingKeyFormatter(context => context.Message.GetType().Name.Underscore());
            });
    }
}
