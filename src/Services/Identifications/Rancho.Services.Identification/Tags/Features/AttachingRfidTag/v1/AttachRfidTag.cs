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

namespace Rancho.Services.Identification.Tags.Features.AttachingRfidTag.v1;

public record AttachRfidTag : IUpdateCommand<Unit>
{
    public Guid RfidTagId { get; set; }
    public Guid FarmId { get; set; }
    public string EarTagNumber { get; set; }
}

public class AttachRfidTagHandler : ICommandHandler<AttachRfidTag, Unit>
{
    private readonly IIdentificationDbContext _context;

    public AttachRfidTagHandler(IIdentificationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AttachRfidTag request, CancellationToken cancellationToken)
    {
        var tag = await _context.RfidTags
                         .Include(x => x.Animal)
                         .FirstOrDefaultAsync(x => x.Id == request.RfidTagId, cancellationToken: cancellationToken);
        Guard.Against.Null(tag, nameof(tag));

        var animal = await _context.Animals
                         .FirstOrDefaultAsync(
                             x => x.EarTagNumber == request.EarTagNumber && x.FarmId == request.FarmId,
                             cancellationToken: cancellationToken);
        Guard.Against.Null(animal);

        tag.Status = Status.Attached;
        tag.AnimalId = animal.Id;
        animal.RfidTagId = tag.Id;

        _context.RfidTags.Update(tag);
        _context.Animals.Update(animal);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class AttachRfidTagEndpoint : EndpointBaseAsync.WithRequest<AttachRfidTag>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public AttachRfidTagEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(RfidTagConfig.TagsPrefixUri + "/attach", Name = "AttachRfidTag")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Attaching Rfid Tag",
        Description = "Attaching Rfid Tag",
        OperationId = "AttachRfidTag",
        Tags = new[]
               {
                   RfidTagConfig.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] AttachRfidTag request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
