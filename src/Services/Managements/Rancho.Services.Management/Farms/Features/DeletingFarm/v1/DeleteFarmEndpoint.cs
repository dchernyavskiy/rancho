using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farms.Features.DeletingFarm.v1;

public class DeleteFarmEndpoint : EndpointBaseAsync.WithRequest<DeleteFarm>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DeleteFarmEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(FarmConfigs.FarmsPrefixUri, Name = "DeleteFarm")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Delete Farm",
        Description = "Delete Farm",
        OperationId = "DeleteFarm",
        Tags = new[]
               {
                   FarmConfigs.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] DeleteFarm request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
