using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rancho.Services.Management.Animals;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1;
using Swashbuckle.AspNetCore.Annotations;

public class CreateAnimalEndpoint : EndpointBaseAsync.WithRequest<CreateAnimal>.WithActionResult<CreateAnimalResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateAnimalEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(AnimalConfigs.AnimalsPrefixUri, Name = "CreateAnimal")]
    [ProducesResponseType(typeof(CreateAnimalResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Create Animal",
        Description = "Create Animal",
        OperationId = "CreateAnimal",
        Tags = new[]
               {
                   AnimalConfigs.Tag
               })]
    public override async Task<ActionResult<CreateAnimalResponse>> HandleAsync(
        [FromBody] CreateAnimal request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _commandProcessor.SendAsync(request, cancellationToken);
        return result;
    }
}
