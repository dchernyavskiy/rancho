using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Features.UpdatingAnimal.v1;
using Rancho.Services.Management.Shared.Contracts;

namespace Rancho.Services.Management.Farms.Features.ChangingOwner.v1;

public record ChangeOwner : IUpdateCommand
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
}

public class ChangeOwnerHandler : ICommandHandler<ChangeOwner, Unit>
{
    private readonly IManagementDbContext _context;


    public ChangeOwnerHandler(IManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ChangeOwner request, CancellationToken cancellationToken)
    {
        var entity = await _context.Farms
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Farm was not found.");

        entity.OwnerId = request.OwnerId;

        _context.Farms.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
