using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Shared.Contracts;
using Rancho.Services.Identification.Tags.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Identification.Tags.Features.DetachingRfidTag.v1;

public record DetachRfidTag : IUpdateCommand<Unit>
{
    public Guid RfidTagId { get; set; }
}

public class DetachRfidTagHandler : ICommandHandler<DetachRfidTag, Unit>
{
    private readonly IIdentificationDbContext _context;

    public DetachRfidTagHandler(IIdentificationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DetachRfidTag request, CancellationToken cancellationToken)
    {
        var entity = await _context.RfidTags
                         .Include(x => x.Animal)
                         .FirstOrDefaultAsync(x => x.Id == request.RfidTagId, cancellationToken: cancellationToken);
        Guard.Against.Null(entity, nameof(entity));

        entity.Status = Status.Detached;
        entity.AnimalId = null;
        entity.Animal.RfidTagId = null;

        _context.RfidTags.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class DetachRfidTagEndpoint : EndpointBaseAsync.WithRequest<DetachRfidTag>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DetachRfidTagEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(RfidTagConfig.TagsPrefixUri + "/detach", Name = "DetachRfidTag")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Detaching Rfid Tag",
        Description = "Detaching Rfid Tag",
        OperationId = "DetachRfidTag",
        Tags = new[]
               {
                   RfidTagConfig.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] DetachRfidTag request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
