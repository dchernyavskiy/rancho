using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rancho.Services.Feeding.Feeds;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.FeedPlans.Features.UpdatingFeedPlan.v1;

public class UpdateFeedPlanEndpoint : EndpointBaseAsync.WithRequest<UpdateFeedPlan>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public UpdateFeedPlanEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(FeedPlanConfigs.FeedPlansPrefixUri, Name = "UpdateFeedPlan")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Update Feed Plan",
        Description = "Update Feed Plan",
        OperationId = "UpdateFeedPlan",
        Tags = new[]
               {
                   FeedPlanConfigs.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] UpdateFeedPlan request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
