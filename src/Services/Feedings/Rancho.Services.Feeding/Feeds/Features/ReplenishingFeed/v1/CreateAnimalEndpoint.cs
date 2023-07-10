using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.Feeds.Features.ReplenishingFeed.v1;

public class CreateAnimalEndpoint : EndpointBaseAsync.WithRequest<ReplenishFeed>.WithActionResult<ReplenishFeedResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateAnimalEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(FeedConfigs.FeedsPrefixUri + "/replenish", Name = "ReplenishFeed")]
    [ProducesResponseType(typeof(ReplenishFeedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Replenish Feed",
        Description = "Replenish Feed",
        OperationId = "ReplenishFeed",
        Tags = new[]
               {
                   FeedConfigs.Tag
               })]
    public override async Task<ActionResult<ReplenishFeedResponse>> HandleAsync(
        [FromBody] ReplenishFeed request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _commandProcessor.SendAsync(request, cancellationToken);
        return result;
    }
}
