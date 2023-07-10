using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.Feeds.Features.GettingFeeds.v1;

public record GetFeeds() : IQuery<GetFeedsResponse>;

public class GetFeedsHandler : IQueryHandler<GetFeeds, GetFeedsResponse>
{
    private readonly IFeedingDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public GetFeedsHandler(IFeedingDbContext context, ISecurityContextAccessor securityContextAccessor)
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
    }

    public async Task<GetFeedsResponse> Handle(GetFeeds request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_securityContextAccessor.UserId);
        var feeds = await _context.Farms
                        .Where(x => x.OwnerId == userId)
                        .SelectMany(x => x.Feeds)
                        .ToListAsync(cancellationToken: cancellationToken);

        return new GetFeedsResponse(feeds);
    }
}

public record GetFeedsResponse(ICollection<Feed> Feeds);

public class GetFeedsEndpoint : EndpointBaseAsync.WithRequest<GetFeeds>.WithoutResult
{
    private readonly IQueryProcessor _queryProcessor;

    public GetFeedsEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(FeedConfigs.FeedsPrefixUri + "/get-all", Name = "GetFeeds")]
    [ProducesResponseType(typeof(GetFeedsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get Feeds",
        Description = "Get Feeds",
        OperationId = "GetFeeds",
        Tags = new[]
               {
                   FeedConfigs.Tag
               })]
    public override async Task<GetFeedsResponse> HandleAsync(
        [FromQuery] GetFeeds request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
