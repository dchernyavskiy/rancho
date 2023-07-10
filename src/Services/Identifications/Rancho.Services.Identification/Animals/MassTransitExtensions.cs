using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Rancho.Services.Identification.Animals.Features.CreatingAnimal.v1.Events.Integration.External;
using Rancho.Services.Identification.Animals.Features.DeletingAnimal.v1.Events.Integration.External;
using Rancho.Services.Identification.Animals.Features.UpdatingAnimal.v1.Events.Integration.External;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Identification.Animals;

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

    internal static void AddAnimalEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint(
            nameof(AnimalCreatedV1).Underscore().Prefixify(),
            re =>
            {
                re.ConfigureConsumeTopology = true;
                re.SetQuorumQueue();
                re.Bind(
                    $"{nameof(AnimalCreatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(AnimalCreatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Fanout;
                    });
                re.ConfigureConsumer<AnimalCreatedConsumer>(context);

                re.RethrowFaultedMessages();
            });

        cfg.ReceiveEndpoint(
            nameof(AnimalUpdatedV1).Underscore().Prefixify(),
            re =>
            {
                re.ConfigureConsumeTopology = true;
                re.SetQuorumQueue();
                re.Bind(
                    $"{nameof(AnimalUpdatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(AnimalUpdatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Fanout;
                    });
                re.ConfigureConsumer<AnimalUpdatedConsumer>(context);

                re.RethrowFaultedMessages();
            });

        cfg.ReceiveEndpoint(
            nameof(AnimalDeletedV1).Underscore().Prefixify(),
            re =>
            {
                re.ConfigureConsumeTopology = true;
                re.SetQuorumQueue();
                re.Bind(
                    $"{nameof(AnimalDeletedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(AnimalDeletedV1).Underscore();
                        e.ExchangeType = ExchangeType.Fanout;
                    });
                re.ConfigureConsumer<AnimalDeletedConsumer>(context);

                re.RethrowFaultedMessages();
            });
    }
}
