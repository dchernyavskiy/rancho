using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Management.Animals;

public static class MassTransitExtensions
{
    internal static void AddAnimalPublishers(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Message<AnimalCreatedV1>(e => e.SetEntityName($"{nameof(AnimalCreatedV1).Underscore()}.input_exchange"));
        cfg.Publish<AnimalCreatedV1>(e => e.ExchangeType = ExchangeType.Fanout);
        cfg.Send<AnimalCreatedV1>(
            e =>
            {
                e.UseRoutingKeyFormatter(context => context.Message.GetType().Name.Underscore());
            });
    }
}
