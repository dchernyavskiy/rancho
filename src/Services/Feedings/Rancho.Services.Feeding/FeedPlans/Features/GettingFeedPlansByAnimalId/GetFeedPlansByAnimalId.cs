using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.FeedPlans.Models;
using Rancho.Services.Feeding.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.FeedPlans.Features.GettingFeedPlansByAnimalId;

public record GetFeedPlansByAnimalId(Guid AnimalId) : IQuery<GetFeedPlansByAnimalIdResponse>;

public record GetFeedPlansByAnimalIdResponse(ICollection<FeedPlan> animalFeedPlans);

public class GetFeedPlansByAnimalIdHandler : IQueryHandler<GetFeedPlansByAnimalId, GetFeedPlansByAnimalIdResponse>
{
    private readonly IFeedingDbContext _context;

    public GetFeedPlansByAnimalIdHandler(IFeedingDbContext context)
    {
        _context = context;
    }

    public async Task<GetFeedPlansByAnimalIdResponse> Handle(
        GetFeedPlansByAnimalId request,
        CancellationToken cancellationToken
    )
    {
        var animal = await _context.Animals
                         .Include(x => x.FeedPlans)
                         .ThenInclude(x => x.Feed)
                         .FirstOrDefaultAsync(x => x.Id == request.AnimalId, cancellationToken: cancellationToken);
        Guard.Against.Null(animal);

        return new GetFeedPlansByAnimalIdResponse(animal.FeedPlans);
    }
}

public class GetFeedPlansByAnimalIdEndpoint : EndpointBaseAsync.WithRequest<GetFeedPlansByAnimalId>.WithResult<
    GetFeedPlansByAnimalIdResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetFeedPlansByAnimalIdEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(FeedPlanConfigs.FeedPlansPrefixUri + "/get-all", Name = "GetFeedPlansByFeedPlanId")]
    [ProducesResponseType(typeof(GetFeedPlansByAnimalIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get List Of FeedPlans",
        Description = "Get List Of FeedPlans",
        OperationId = "GetFeedPlansByFeedPlanId",
        Tags = new[]
               {
                   FeedPlanConfigs.Tag
               })]
    public override async Task<GetFeedPlansByAnimalIdResponse> HandleAsync(
        GetFeedPlansByAnimalId request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
