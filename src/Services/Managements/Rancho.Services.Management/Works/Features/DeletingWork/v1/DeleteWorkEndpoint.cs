using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rancho.Services.Management.Works;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Animals.Features.DeletingWork.v1;

public class DeleteWorkEndpoint : EndpointBaseAsync.WithRequest<DeleteWork>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DeleteWorkEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(WorkConfigs.WorksPrefixUri, Name = "DeleteWork")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Create Work",
        Description = "Create Work",
        OperationId = "DeleteWork",
        Tags = new[]
               {
                   WorkConfigs.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] DeleteWork request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
