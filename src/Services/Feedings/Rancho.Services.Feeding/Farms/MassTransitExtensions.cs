using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Rancho.Services.Feeding.Farms.Features.CreatingFarm.v1.Events.Integration.External;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;

namespace Rancho.Services.Feeding.Farms;

public static class MassTransitExtensions
{
    internal static void AddFarmEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint(
            nameof(FarmCreatedV1).Underscore().Prefixify(),
            re =>
            {
                re.ConfigureConsumeTopology = true;
                re.SetQuorumQueue();
                re.Bind(
                    $"{nameof(FarmCreatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(FarmCreatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Fanout;
                    });
                re.ConfigureConsumer<FarmCreatedConsumer>(
                    context,
                    c =>
                    {
                        c.UseConcurrencyLimit(100);
                        c.UseConcurrentMessageLimit(100);
                    });

                re.RethrowFaultedMessages();
            });
    }
}
