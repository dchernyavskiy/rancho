using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Shared.Contracts;
using Rancho.Services.Identification.Tags;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Identification.Tags.Features.DeletingRfidTag.v1;

public record DeleteRfidTag() : IDeleteCommand<Guid>
{
    public Guid Id { get; init; }
}

public class DeleteRfidTagValidator : AbstractValidator<DeleteRfidTag>
{
    public DeleteRfidTagValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteRfidTagHandler : ICommandHandler<DeleteRfidTag>
{
    private readonly IIdentificationDbContext _context;

    public DeleteRfidTagHandler(IIdentificationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRfidTag request, CancellationToken cancellationToken)
    {
        var tag = await _context.RfidTags.FirstOrDefaultAsync(
                      x => x.Id == request.Id,
                      cancellationToken: cancellationToken);

        Guard.Against.Null(tag, "Tag was not found.");

        _context.RfidTags.Remove(tag);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

public class DeleteRfidTagEndpoint : EndpointBaseAsync.WithRequest<DeleteRfidTag>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DeleteRfidTagEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(RfidTagConfig.TagsPrefixUri, Name = "DeleteRfidTag")]
    [ProducesResponseType(typeof(StatusCodeResult), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Delete RFID Tag",
        Description = "Deleting RFID Tag",
        OperationId = "DeleteRfidTag",
        Tags = new[]
               {
                   RfidTagConfig.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] DeleteRfidTag request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
