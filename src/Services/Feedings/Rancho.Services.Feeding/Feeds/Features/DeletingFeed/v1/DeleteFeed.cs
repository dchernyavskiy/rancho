using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Feeding.Feeds.Features.DeletingFeed.v1;

public record DeleteFeed : IDeleteCommand<Guid>
{
    public Guid Id { get; init; }
}

public class DeleteFeedHandler : ICommandHandler<DeleteFeed>
{
    private readonly IFeedingDbContext _context;

    public DeleteFeedHandler(IFeedingDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteFeed request, CancellationToken cancellationToken)
    {
        var feed = await _context.Feeds.FirstOrDefaultAsync(
                       x => x.Id == request.Id,
                       cancellationToken: cancellationToken);
        Guard.Against.Null(feed);

        _context.Feeds.Remove(feed);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

public class DeleteFeedEndpoint : EndpointBaseAsync.WithRequest<DeleteFeed>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DeleteFeedEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(FeedConfigs.FeedsPrefixUri, Name = "DeleteFeed")]
    [ProducesResponseType(typeof(DeleteFeed), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Delete Feed",
        Description = "Delete Feed",
        OperationId = "DeleteFeed",
        Tags = new[]
               {
                   FeedConfigs.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] DeleteFeed request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}

