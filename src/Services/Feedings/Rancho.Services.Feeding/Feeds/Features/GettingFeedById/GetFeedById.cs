using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.Feeds.Features.GettingFeedById;

public record GetFeedById(Guid Id) : IQuery<GetFeedByIdResponse>;

public record GetFeedByIdResponse(Feed Feed);

public class GetFeedByIdHandler : IQueryHandler<GetFeedById, GetFeedByIdResponse>
{
    private readonly IFeedingDbContext _context;

    public GetFeedByIdHandler(IFeedingDbContext context)
    {
        _context = context;
    }

    public async Task<GetFeedByIdResponse> Handle(GetFeedById request, CancellationToken cancellationToken)
    {
        var feed = await _context.Feeds.FirstOrDefaultAsync(
                       x => x.Id == request.Id,
                       cancellationToken: cancellationToken);
        return new GetFeedByIdResponse(feed!);
    }
}

public class GetFeedByIdEndpoint : EndpointBaseAsync.WithRequest<GetFeedById>.WithoutResult
{
    private readonly IQueryProcessor _queryProcessor;

    public GetFeedByIdEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(FeedConfigs.FeedsPrefixUri, Name = "GetFeedById")]
    [ProducesResponseType(typeof(GetFeedByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Get Feeds",
        Description = "Get Feeds",
        OperationId = "GetFeedById",
        Tags = new[]
               {
                   FeedConfigs.Tag
               })]
    public override async Task<GetFeedByIdResponse> HandleAsync(
        [FromQuery] GetFeedById request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _queryProcessor.SendAsync(request, cancellationToken);
    }
}
