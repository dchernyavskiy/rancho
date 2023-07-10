using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Animals.Features.DeletingAnimal.v1;

public class DeleteAnimalEndpoint : EndpointBaseAsync.WithRequest<DeleteAnimal>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DeleteAnimalEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(AnimalConfigs.AnimalsPrefixUri, Name = "DeleteAnimal")]
    [ProducesResponseType(typeof(DeleteAnimal), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Delete Animal",
        Description = "Delete Animal",
        OperationId = "DeleteAnimal",
        Tags = new[]
               {
                   AnimalConfigs.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] DeleteAnimal request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
