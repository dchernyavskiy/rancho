using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Core.Exception.Types;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Animals;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Shared.Contracts;

namespace Rancho.Services.Feeding.Feeds.Features.ReplenishingFeed.v1;

public record ReplenishFeed(Guid Id, decimal Weight) : ICommand<ReplenishFeedResponse>;

public record ReplenishFeedResponse(Feed Feed);

public class ReplenishFeedValidator : AbstractValidator<ReplenishFeed>
{
    public ReplenishFeedValidator(IFeedingDbContext context)
    {
        //RuleFor(x => x.Weight).GreaterThan(0m).WithMessage("Weight must be greater than 0");
    }
}

public class ReplenishFeedHandler : ICommandHandler<ReplenishFeed, ReplenishFeedResponse>
{
    private readonly IFeedingDbContext _context;

    public ReplenishFeedHandler(IFeedingDbContext context)
    {
        _context = context;
    }

    public async Task<ReplenishFeedResponse> Handle(ReplenishFeed request, CancellationToken cancellationToken)
    {
        var entity = await _context.Feeds
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        Guard.Against.Null(entity, nameof(entity) + " not found");

        if (entity.WeightInStock + request.Weight < 0)
            throw new CustomException("The weight indicated is greater than what is in stock");

        entity.WeightInStock = request.Weight;

        await _context.SaveChangesAsync(cancellationToken);

        return new ReplenishFeedResponse(entity);
    }
}
