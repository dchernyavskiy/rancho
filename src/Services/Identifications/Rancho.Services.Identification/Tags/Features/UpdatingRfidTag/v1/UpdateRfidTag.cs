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

namespace Rancho.Services.Identification.Tags.Features.UpdatingRfidTag.v1;

public record UpdateRfidTag : IUpdateCommand<Unit>
{
    public Guid Id { get; set; }
    public Status Status { get; set; }
    public Guid AnimalId { get; set; }
}

public class UpdateRfidTagHandler : ICommandHandler<UpdateRfidTag, Unit>
{
    private readonly IIdentificationDbContext _context;

    public UpdateRfidTagHandler(IIdentificationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateRfidTag request, CancellationToken cancellationToken)
    {
        var animal = await _context.Animals
                         .FirstOrDefaultAsync(x => x.Id == request.AnimalId, cancellationToken: cancellationToken);

        Guard.Against.Null(animal, "Animal was not found.");

        var entity = await _context.RfidTags
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Tag was not found.");

        entity.AnimalId = request.AnimalId;
        entity.Status = request.Status;

        _context.RfidTags.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class UpdateRfidTagEndpoint : EndpointBaseAsync.WithRequest<UpdateRfidTag>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public UpdateRfidTagEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpPut(RfidTagConfig.TagsPrefixUri, Name = "UpdateRfidTag")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Update Rfid Tag",
        Description = "Update Rfid Tag",
        OperationId = "UpdateRfidTag",
        Tags = new[]
               {
                   RfidTagConfig.Tag
               })]
    public override async Task<NoContentResult> HandleAsync(
        [FromBody] UpdateRfidTag request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
