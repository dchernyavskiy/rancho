using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Works.Features.CreatingWork.v1;

public class CreateWorkEndpoint : EndpointBaseAsync.WithRequest<CreateWork>.WithActionResult<CreateWorkResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    public CreateWorkEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(WorkConfigs.WorksPrefixUri, Name = "CreateWork")]
    [ProducesResponseType(typeof(CreateWorkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Create Work",
        Description = "Create Work",
        OperationId = "CreateWork",
        Tags = new[]
               {
                   WorkConfigs.Tag
               })]
    public override async Task<ActionResult<CreateWorkResponse>> HandleAsync(
        [FromBody] CreateWork request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        var result = await _commandProcessor.SendAsync(request, cancellationToken);
        return result;
    }
}
