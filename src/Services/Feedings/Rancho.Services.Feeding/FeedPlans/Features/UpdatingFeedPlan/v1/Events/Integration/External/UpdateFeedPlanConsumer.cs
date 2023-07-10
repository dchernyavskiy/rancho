using BuildingBlocks.Core.Messaging;
using MassTransit;
using MediatR;

namespace Rancho.Services.Feeding.FeedPlans.Features.UpdatingFeedPlan.v1.Events.Integration.External;

public record UpdateFeedV1(Guid FeedId, Guid AnimalId, decimal WeightEaten) : IntegrationEvent;

public class UpdateFeedPlanConsumer : IConsumer<UpdateFeedV1>
{
    private readonly ISender _sender;

    public UpdateFeedPlanConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<UpdateFeedV1> context)
    {
        try
        {
            await _sender.Send(
                new UpdateFeedPlan(
                    context.Message.FeedId,
                    context.Message.AnimalId,
                    context.Message.WeightEaten));
        }
        catch
        {

        }
    }
}
