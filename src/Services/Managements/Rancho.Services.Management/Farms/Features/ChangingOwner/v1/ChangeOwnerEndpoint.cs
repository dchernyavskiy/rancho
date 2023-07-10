using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farms.Features.ChangingOwner.v1;

public class ChangeOwnerEndpoint : EndpointBaseAsync.WithRequest<ChangeOwner>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public ChangeOwnerEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(FarmConfigs.FarmsPrefixUri + "/change-owner", Name = "ChangeOwner")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Changing Owner",
        Description = "Changing Owner",
        OperationId = "ChangeOwner",
        Tags = new[]
               {
                   FarmConfigs.Tag,
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] ChangeOwner request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
