using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Queries;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Animals.Features.GettingAnimalByEarTagNumber.v1;

public class GetAnimalByEarTagNumberEndpoint : EndpointBaseAsync.WithRequest<GetAnimalByEarTagNumber>.WithActionResult
    <GetAnimalByEarTagNumberResponse>
{
    private readonly IQueryProcessor _queryProcessor;

    public GetAnimalByEarTagNumberEndpoint(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet(AnimalConfigs.AnimalsPrefixUri + "/get-by-ear-tag-number", Name = "GetAnimalByEarTag")]
    [ProducesResponseType(typeof(GetAnimalByEarTagNumberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Getting Animal",
        Description = "Getting Animal By Ear Tag",
        OperationId = "GetAnimalByEarTag",
        Tags = new[]
               {
                   AnimalConfigs.Tag
               })]
    public override async Task<ActionResult<GetAnimalByEarTagNumberResponse>> HandleAsync(
        [FromQuery] GetAnimalByEarTagNumber request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var collection = await _queryProcessor.SendAsync(request, cancellationToken);
        return collection;
    }
}
