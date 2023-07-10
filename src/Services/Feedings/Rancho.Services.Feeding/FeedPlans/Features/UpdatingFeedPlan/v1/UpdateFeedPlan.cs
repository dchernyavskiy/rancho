using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.FeedPlans.Models;
using Rancho.Services.Feeding.Shared.Contracts;

namespace Rancho.Services.Feeding.FeedPlans.Features.UpdatingFeedPlan.v1;

public record UpdateFeedPlan(Guid FeedId, Guid AnimalId, decimal WeightEaten) : ITxCommand<UpdateFeedPlanResponse>;

public record UpdateFeedPlanResponse(FeedPlan FeedPlan);

public class UpdateFeedPlanValidator : AbstractValidator<UpdateFeedPlan>
{
}

public class UpdateFeedPlanHandler : ICommandHandler<UpdateFeedPlan, UpdateFeedPlanResponse>
{
    private readonly IFeedingDbContext _context;

    public UpdateFeedPlanHandler(IFeedingDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateFeedPlanResponse> Handle(UpdateFeedPlan request, CancellationToken cancellationToken)
    {
        var entity = await _context.FeedPlans
                         .Include(x => x.Feed)
                         .FirstOrDefaultAsync(
                             x => x.FeedId == request.FeedId &&
                                  x.AnimalId == request.AnimalId &&
                                  x.DispenseDate.CompareTo(DateTime.Today) >= 0,
                             cancellationToken);

        Guard.Against.Null(entity, "entity != null");

        entity.WeightEaten = request.WeightEaten;
        entity.Feed.WeightInStock -= request.WeightEaten;

        _context.FeedPlans.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateFeedPlanResponse(entity);
    }
}
