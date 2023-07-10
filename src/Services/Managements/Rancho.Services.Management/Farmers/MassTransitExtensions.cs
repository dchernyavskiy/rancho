using Humanizer;
using MassTransit;
using RabbitMQ.Client;
using Rancho.Services.Management.Farmers.Features.CreatingFarmer.v1.Events.Integration.External;
using Rancho.Services.Management.Farmers.Features.DeletingFarmer.v1.Events.Integration.External;
using Rancho.Services.Management.Farmers.Features.UpdatingFarmer.v1.Events.Integration.External;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

namespace Rancho.Services.Management.Farmers;

internal static class MassTransitExtensions
{
    internal static void AddFarmerEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        // cfg.ReceiveEndpoint(
        //     nameof(FarmerCreatedV1).Underscore(),
        //     re =>
        //     {
        //         re.ConfigureConsumeTopology = true;
        //         re.SetQuorumQueue();
        //         re.Bind(
        //             $"{nameof(FarmerCreatedV1).Underscore()}.input_exchange",
        //             e =>
        //             {
        //                 e.RoutingKey = nameof(FarmerCreatedV1).Underscore();
        //                 e.ExchangeType = ExchangeType.Fanout;
        //             });
        //         re.ConfigureConsumer<FarmerCreatedConsumer>(context);
        //
        //         re.RethrowFaultedMessages();
        //     });

        cfg.ReceiveEndpoint(
            nameof(FarmerDeletedV1).Underscore().Prefixify(),
            re =>
            {
                re.ConfigureConsumeTopology = true;
                re.SetQuorumQueue();
                re.Bind(
                    $"{nameof(FarmerDeletedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(FarmerDeletedV1).Underscore();
                        e.ExchangeType = ExchangeType.Fanout;
                    });
                re.ConfigureConsumer<FarmerDeletedConsumer>(context);

                re.RethrowFaultedMessages();
            });

        cfg.ReceiveEndpoint(
            nameof(FarmerUpdatedV1).Underscore().Prefixify(),
            re =>
            {
                re.ConfigureConsumeTopology = true;
                re.SetQuorumQueue();
                re.Bind(
                    $"{nameof(FarmerUpdatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(FarmerUpdatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Fanout;
                    });
                re.ConfigureConsumer<FarmerUpdatedConsumer>(context);

                re.RethrowFaultedMessages();
            });
    }
}
