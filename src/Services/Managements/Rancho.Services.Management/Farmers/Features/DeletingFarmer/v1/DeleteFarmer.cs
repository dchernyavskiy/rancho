using Ardalis.ApiEndpoints;
using Ardalis.GuardClauses;
using Asp.Versioning;
using BuildingBlocks.Abstractions.CQRS.Commands;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Rancho.Services.Management.Farmers.Features.DeletingFarmer.v1;

public record DeleteFarmer(Guid Id) : IDeleteCommand<Guid>;

public class DeleteFarmerHandler : ICommandHandler<DeleteFarmer>
{
    private readonly IManagementDbContext _context;

    public DeleteFarmerHandler(IManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteFarmer request, CancellationToken cancellationToken)
    {
        var farmer = await _context.Farmers.FirstOrDefaultAsync(x => x.Id == request.Id);
        Guard.Against.Null(farmer, "Farmer not found");

        _context.Farmers.Remove(farmer);
        return Unit.Value;
    }
}

public class DeleteFarmerEndpoint : EndpointBaseAsync.WithRequest<DeleteFarmer>.WithoutResult
{
    private readonly ICommandProcessor _commandProcessor;

    public DeleteFarmerEndpoint(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [HttpDelete(FarmerConfigs.FarmersPrefixUri, Name = "DeleteFarmer")]
    [ProducesResponseType(typeof(DeleteFarmer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Delete Farmer",
        Description = "Delete Farmer",
        OperationId = "DeleteFarmer",
        Tags = new[]
               {
                   FarmerConfigs.Tag
               })]
    public override async Task<IActionResult> HandleAsync(
        [FromBody] DeleteFarmer request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _commandProcessor.SendAsync(request, cancellationToken);
        return NoContent();
    }
}
