using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Animals;
using Rancho.Services.Management.Shared.Contracts;

namespace Rancho.Services.Management.Farms.Features.DeletingFarm.v1;

public record DeleteFarm(Guid Id)
    : IDeleteCommand<Guid>;

public class DeleteFarmHandler : ICommandHandler<DeleteFarm>
{
    private readonly IManagementDbContext _context;
    private readonly ILogger<DeleteFarmHandler> _logger;

    public DeleteFarmHandler(IManagementDbContext context, ILogger<DeleteFarmHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteFarm request, CancellationToken cancellationToken)
    {
        var entity = await _context.Farms
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Farm was not found");

        _context.Farms.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Farm with id {entity.Id} was deleted.");

        return Unit.Value;
    }
}
