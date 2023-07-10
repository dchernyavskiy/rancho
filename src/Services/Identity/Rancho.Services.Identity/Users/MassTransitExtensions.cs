using Humanizer;
using MassTransit;
using Microsoft.VisualBasic;
using RabbitMQ.Client;
using Rancho.Services.Identity.Users.Consumers;
using Rancho.Services.Identity.Users.Features.UpdatingUserState.v1.Events.Integration;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

namespace Rancho.Services.Identity.Users;

internal static class MassTransitExtensions
{
    internal static void AddUserPublishers(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Message<UserRegisteredV1>(
            e => e.SetEntityName(
                $"{nameof(UserRegisteredV1).Underscore()}.input_exchange")); // name of the primary exchange
        cfg.Publish<UserRegisteredV1>(e => e.ExchangeType = ExchangeType.Fanout); // primary exchange type
        cfg.Send<UserRegisteredV1>(
            e =>
            {
                // route by message type to binding fanout exchange (exchange to exchange binding)
                e.UseRoutingKeyFormatter(context => context.Message.GetType().Name.Underscore());
            });

        cfg.Message<UserStateUpdated>(
            e => e.SetEntityName(
                $"{nameof(UserStateUpdated).Underscore()}.input_exchange")); // name of the primary exchange
        cfg.Publish<UserStateUpdated>(e => e.ExchangeType = ExchangeType.Fanout); // primary exchange type
        cfg.Send<UserStateUpdated>(
            e =>
            {
                // route by message type to binding fanout exchange (exchange to exchange binding)
                e.UseRoutingKeyFormatter(context => context.Message.GetType().Name.Underscore());
            });
    }

    internal static void AddFarmerEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint(
            nameof(FarmerCreatedV1).Underscore() + "_identity",
            re =>
            {
                re.ConfigureConsumeTopology = true;
                re.SetQuorumQueue();
                re.Bind(
                    $"{nameof(FarmerCreatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(FarmerCreatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Fanout;
                    });
                re.ConfigureConsumer<FarmerCreatedConsumer>(context);

                re.RethrowFaultedMessages();
            });
    }
}
