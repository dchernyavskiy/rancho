using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Animals.Features.GettingAnimalById.v1;

public class GetAnimalByIdEndpoint : EndpointBaseAsync.WithRequest<GetAnimalById>.WithActionResult
    <GetAnimalByIdResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetAnimalByIdEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(AnimalConfigs.AnimalsPrefixUri, Name = "GetAnimalById")]
    [ProducesResponseType(typeof(GetAnimalByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Getting Animal",
        Description = "Getting Animal By Id",
        OperationId = "GetAnimalById",
        Tags = new[]
               {
                   AnimalConfigs.Tag
               })]
    public override async Task<ActionResult<GetAnimalByIdResponse>> HandleAsync(
        [FromQuery] GetAnimalById request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var collection = await _queryProcessor.SendAsync(request, cancellationToken);
        return collection;
    }
}
