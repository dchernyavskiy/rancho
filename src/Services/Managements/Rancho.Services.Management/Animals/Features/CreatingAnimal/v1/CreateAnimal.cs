using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Rancho.Services.Management.Animals.Dtos;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

namespace Rancho.Services.Management.Animals.Features.CreatingAnimal.v1;

public record CreateAnimal
(
    string Species,
    DateTime BirthDate,
    Gender Gender,
    string EarTagNumber,
    Guid FarmId
) : ITxCreateCommand<CreateAnimalResponse>;

public class CreateAnimalValidator : AbstractValidator<CreateAnimal>
{
    public CreateAnimalValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Species).NotEmpty().NotNull().MinimumLength(2);
    }
}

public class CreateAnimalHandler : ICommandHandler<CreateAnimal, CreateAnimalResponse>
{
    private readonly ILogger<CreateAnimalHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IManagementDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IMessagePersistenceService _messagePersistenceService;

    public CreateAnimalHandler(
        ILogger<CreateAnimalHandler> logger,
        IMapper mapper,
        IManagementDbContext context,
        ISecurityContextAccessor securityContextAccessor,
        IMessagePersistenceService messagePersistenceService
    )
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _securityContextAccessor = securityContextAccessor;
        _messagePersistenceService = messagePersistenceService;
    }

    public async Task<CreateAnimalResponse> Handle(CreateAnimal request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var userId = Guid.Parse(_securityContextAccessor.UserId);

        var farm = await _context.Farms
                       .Include(x => x.Farmers)
                       .FirstOrDefaultAsync(
                           x => x.Id == request.FarmId,
                           cancellationToken: cancellationToken);

        Guard.Against.Null(farm, $"User is not registered on farm");

        var animal = new Animal()
                     {
                         Species = request.Species,
                         BirthDate = request.BirthDate,
                         Gender = request.Gender,
                         EarTagNumber = request.EarTagNumber,
                         FarmId = farm.Id
                     };

        await _context.Animals.AddAsync(animal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Animal with Id: {animal.Id} was created");

        var animalDto = _mapper.Map<AnimalDto>(animal);

        await _messagePersistenceService.AddPublishMessageAsync(
            new MessageEnvelope<AnimalCreatedV1>(
                new AnimalCreatedV1(
                    animal.Id,
                    request.Species,
                    request.BirthDate,
                    request.Gender.ToString(),
                    request.EarTagNumber,
                    request.FarmId),
                new Dictionary<string, object?>()),
            cancellationToken);

        return new CreateAnimalResponse(animalDto);
    }
}
