using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rancho.Services.Management.Animals;
using Rancho.Services.Management.Farms.Features.CreatingAnimal.v1;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farms.Features.CreatingFarm.v1;

public class CreateFarmEndpoint : EndpointBaseAsync.WithRequest<CreateFarm>.WithActionResult<CreateFarmResponse>
{
    private readonly ISender _commandProcessor;

    public CreateFarmEndpoint(ISender commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(FarmConfigs.FarmsPrefixUri, Name = "CreateFarm")]
    [ProducesResponseType(typeof(CreateFarmResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Creating Farm",
        Description = "Creating Farm",
        OperationId = "CreateFarm",
        Tags = new[]
               {
                   FarmConfigs.Tag
               })]
    public override async Task<ActionResult<CreateFarmResponse>> HandleAsync(
        [FromBody] CreateFarm request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _commandProcessor.Send(request, cancellationToken);
        return result;
    }
}
