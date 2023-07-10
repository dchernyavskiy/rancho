using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rancho.Services.Management.Animals.Features.DeletingAnimal.v1;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Animals.Features.UpdatingAnimal.v1;

public class UpdateAnimalEndpoint : EndpointBaseAsync.WithRequest<UpdateAnimal>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public UpdateAnimalEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(AnimalConfigs.AnimalsPrefixUri, Name = "UpdateAnimal")]
    [ProducesResponseType(typeof(UpdateAnimal), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Update Animal",
        Description = "Update Animal",
        OperationId = "UpdateAnimal",
        Tags = new[]
               {
                   AnimalConfigs.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] UpdateAnimal request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
