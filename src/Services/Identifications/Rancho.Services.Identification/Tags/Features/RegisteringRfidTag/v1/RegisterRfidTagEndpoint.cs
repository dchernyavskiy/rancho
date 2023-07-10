using Ardalis.ApiEndpoints;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rancho.Services.Identification.Tags;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Identification.Tags.Features.RegisteringRfidTag.v1;

public class
    RegisterRfidTagEndpoint : EndpointBaseAsync.WithRequest<RegisterRfidTag>.WithActionResult<RegisterRfidTagResponse>
{
    private readonly ICommandProcessor _commandProcessor;

    public RegisterRfidTagEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPost(RfidTagConfig.TagsPrefixUri, Name = "RegisterRfidTag")]
    [ProducesResponseType(typeof(RegisterRfidTagResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Registering Rfid Tag",
        Description = "Registering Rfid Tag",
        OperationId = "RegisterRfidTag",
        Tags = new[]
               {
                   RfidTagConfig.Tag
               })]
    public override async Task<ActionResult<RegisterRfidTagResponse>> HandleAsync(
        [FromBody] RegisterRfidTag request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return await _commandProcessor.SendAsync(request, cancellationToken);
    }
}
