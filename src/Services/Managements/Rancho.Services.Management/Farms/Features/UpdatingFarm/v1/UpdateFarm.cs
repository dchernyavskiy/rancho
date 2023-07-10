using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals;
using Rancho.Services.Management.Animals.Features.UpdatingAnimal.v1;
using Rancho.Services.Management.Shared.Contracts;

namespace Rancho.Services.Management.Farms.Features.UpdatingFarm.v1;

public record UpdateFarm : IUpdateCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
}

public class UpdateFarmHandler : ICommandHandler<UpdateFarm, Unit>
{
    private readonly IManagementDbContext _context;


    public UpdateFarmHandler(IManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateFarm request, CancellationToken cancellationToken)
    {
        var entity = await _context.Farms
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Farm was not found.");

        entity.Name = request.Name;
        entity.Address = request.Address;

        _context.Farms.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
