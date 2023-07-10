using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Bogus.DataSets;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Reports;
using Rancho.Services.Feeding.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.Feeds.Features.GettingFeedsByFarmId.v1;

public record GetFeedsByFarmId(Guid FarmId) : IQuery<GetFeedsByFarmIdResponse>;

public record GetFeedsByFarmIdResponse(ICollection<Feed> Feeds);

public class GetFeedsByFarmIdHandler : IQueryHandler<GetFeedsByFarmId, GetFeedsByFarmIdResponse>
{
    private readonly IFeedingDbContext _context;

    public GetFeedsByFarmIdHandler(IFeedingDbContext context)
    {
        _context = context;
    }

    public async Task<GetFeedsByFarmIdResponse> Handle(GetFeedsByFarmId request, CancellationToken cancellationToken)
    {
        return new GetFeedsByFarmIdResponse(
            await _context.Feeds.Where(x => x.FarmId == request.FarmId)
                .ToListAsync(cancellationToken: cancellationToken));
    }
}

public class
    GetFeedsByFarmIdEndpoint : EndpointBaseAsync.WithRequest<GetFeedsByFarmId>.WithActionResult<
        GetFeedsByFarmIdResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetFeedsByFarmIdEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(FeedConfigs.FeedsPrefixUri + "/by-farm-id", Name = "GetFeedsByFarmId")]
    [ProducesResponseType(typeof(GetFeedsByFarmIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get Feed Report",
        Description = "Get Feed Report",
        OperationId = "GetFeedsByFarmId",
        Tags = new[]
               {
                   FeedConfigs.Tag
               })]
    public override async Task<ActionResult<GetFeedsByFarmIdResponse>> HandleAsync(
        [FromQuery] GetFeedsByFarmId request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _queryProcessor.SendAsync(request, cancellationToken);
        return result;
    }
}
