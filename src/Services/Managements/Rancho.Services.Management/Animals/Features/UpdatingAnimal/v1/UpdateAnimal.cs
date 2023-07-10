using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Mapping;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Management.Animals.Features.UpdatingAnimal.v1;

public record UpdateAnimal : IUpdateCommand<Unit>, IMapWith<Animal>
{
    public Guid Id { get; set; }
    public string Species { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string EarTagNumber { get; set; } = null!;
    public Guid FarmId { get; set; } = Guid.Empty;
}

public class UpdateAnimalHandler : ICommandHandler<UpdateAnimal, Unit>
{
    private readonly IManagementDbContext _context;
    private readonly IMessagePersistenceService _messagePersistenceService;
    private readonly IMapper _mapper;


    public UpdateAnimalHandler(IManagementDbContext context, IMessagePersistenceService messagePersistenceService, IMapper mapper)
    {
        _context = context;
        _messagePersistenceService = messagePersistenceService;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateAnimal request, CancellationToken cancellationToken)
    {
        var animal = await _context.Animals
                         .AsNoTracking()
                         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        Guard.Against.Null(animal, "Animal was not found.");

        animal = _mapper.Map<Animal>(request);
        if (request.FarmId != Guid.Empty) animal.FarmId = request.FarmId;

        _context.Animals.Update(animal);
        await _context.SaveChangesAsync(cancellationToken);

        await _messagePersistenceService.AddPublishMessageAsync(
            new MessageEnvelope<AnimalUpdatedV1>(
                new AnimalUpdatedV1(
                    request.Id,
                    request.Species,
                    request.BirthDate,
                    request.Gender.ToString(),
                    request.EarTagNumber,
                    animal.FarmId),
                new Dictionary<string, object?>()),
            cancellationToken);

        return Unit.Value;
    }
}
