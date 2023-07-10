using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Animals.Features.GettingListOfAnimals.v1;

public class GetListOfAnimalsEndpoint : EndpointBaseAsync.WithRequest<GetListOfAnimals>.WithActionResult
    <GetListOfAnimalsResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetListOfAnimalsEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(AnimalConfigs.AnimalsPrefixUri + "/get-all", Name = "GetAnimals")]
    [ProducesResponseType(typeof(GetListOfAnimalsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Getting All Animals",
        Description = "Getting All Animals",
        OperationId = "GetAnimals",
        Tags = new[]
               {
                   AnimalConfigs.Tag
               })]
    public override async Task<ActionResult<GetListOfAnimalsResponse>> HandleAsync(
        [FromQuery] GetListOfAnimals request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var collection = await _queryProcessor.SendAsync(request, cancellationToken);
        return collection;
    }
}
