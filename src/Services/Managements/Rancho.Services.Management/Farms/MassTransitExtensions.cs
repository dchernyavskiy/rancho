using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;

namespace Rancho.Services.Management.Farms;

public static class MassTransitExtensions
{
    internal static void AddFarmPublishers(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Message<FarmCreatedV1>(e => e.SetEntityName($"{nameof(FarmCreatedV1).Underscore()}.input_exchange"));
        cfg.Publish<FarmCreatedV1>(e => e.ExchangeType = ExchangeType.Fanout);
        cfg.Send<FarmCreatedV1>(
            e =>
            {
                e.UseRoutingKeyFormatter(context => context.Message.GetType().Name.Underscore());
            });
    }
}
