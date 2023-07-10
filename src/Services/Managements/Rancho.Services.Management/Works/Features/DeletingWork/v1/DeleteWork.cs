using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Shared.Contracts;

namespace Rancho.Services.Management.Animals.Features.DeletingWork.v1;

public record DeleteWork(Guid Id)
    : IDeleteCommand<Guid>
{
}

public class DeleteWorkHandler : ICommandHandler<DeleteWork>
{
    private readonly IManagementDbContext _context;

    public DeleteWorkHandler(IManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteWork request, CancellationToken cancellationToken)
    {
        var animal = await _context.Works
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.Null(animal, "Animal was not found");

        _context.Works.Remove(animal);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
