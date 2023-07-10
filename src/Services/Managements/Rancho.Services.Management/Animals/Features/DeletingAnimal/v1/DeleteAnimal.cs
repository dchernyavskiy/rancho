using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Features.CreatingAnimal.v1;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Management.Animals.Features.DeletingAnimal.v1;

public record DeleteAnimal(Guid Id)
    : IDeleteCommand<Guid>
{
}

public class DeleteAnimalValidator : AbstractValidator<DeleteAnimal>
{
    public DeleteAnimalValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteAnimalHandler : ICommandHandler<DeleteAnimal>
{
    private readonly IManagementDbContext _context;
    private readonly IMessagePersistenceService _messagePersistenceService;

    public DeleteAnimalHandler(IManagementDbContext context, IMessagePersistenceService messagePersistenceService)
    {
        _context = context;
        _messagePersistenceService = messagePersistenceService;
    }

    public async Task<Unit> Handle(DeleteAnimal request, CancellationToken cancellationToken)
    {
        var entity = await _context.Animals
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.Null(entity, "Animal was not found");

        _context.Animals.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        await _messagePersistenceService.AddPublishMessageAsync(
            new MessageEnvelope<AnimalDeletedV1>(
                new AnimalDeletedV1(request.Id),
                new Dictionary<string, object?>()),
            cancellationToken);

        return Unit.Value;
    }
}
