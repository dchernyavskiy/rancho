using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farms.Features.UpdatingFarm.v1;

public class UpdateFarmEndpoint : EndpointBaseAsync.WithRequest<UpdateFarm>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public UpdateFarmEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(FarmConfigs.FarmsPrefixUri, Name = "UpdateFarm")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Update Animal",
        Description = "Update Animal",
        OperationId = "UpdateFarm",
        Tags = new[]
               {
                   FarmConfigs.Tag,
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] UpdateFarm request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
