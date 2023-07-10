using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Farms.Features.CreatingAnimal.v1;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;
using IBus = MassTransit.IBus;

namespace Rancho.Services.Management.Farms.Features.CreatingFarm.v1;

public record CreateFarm(
    string Name,
    string Address
) : ICreateCommand<CreateFarmResponse>;

public class CreateFarmValidator : AbstractValidator<CreateFarm>
{
    public CreateFarmValidator()
    {
        CascadeMode = CascadeMode.Stop;
    }
}

public class CreateFarmHandler : ICommandHandler<CreateFarm, CreateFarmResponse>
{
    private readonly IManagementDbContext _context;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IDomainEventPublisher _publisher;
    private readonly ILogger<CreateFarmHandler> _logger;
    private readonly IBus _bus;
    private readonly IMessagePersistenceService _messagePersistenceService;

    public CreateFarmHandler(
        IManagementDbContext context,
        ISecurityContextAccessor securityContextAccessor,
        IDomainEventPublisher publisher,
        ILogger<CreateFarmHandler> logger,
        IBus bus,
        IMessagePersistenceService messagePersistenceService
    )
    {
        _context = context;
        _securityContextAccessor = securityContextAccessor;
        _publisher = publisher;
        _logger = logger;
        _bus = bus;
        _messagePersistenceService = messagePersistenceService;
    }

    public async Task<CreateFarmResponse> Handle(CreateFarm command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var entity = new Farm()
                     {
                         Name = command.Name,
                         Address = command.Address,
                         OwnerId = Guid.Parse(_securityContextAccessor.UserId)
                     };

        await _context.Farms.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _bus.Publish<FarmCreatedV1>(
            new FarmCreatedV1(entity.Id, entity.Name, entity.Address, entity.OwnerId),
            null,
            cancellationToken);

        // await _messagePersistenceService.AddPublishMessageAsync(
        //     new MessageEnvelope<FarmCreatedV1>(
        //         new FarmCreatedV1(
        //             entity.Id,
        //             entity.Name,
        //             entity.Address,
        //             entity.OwnerId),
        //         new Dictionary<string, object?>()),
        //     cancellationToken);

        _logger.LogInformation($"Farm with id {entity.Id} was created successfully.");

        return new CreateFarmResponse(entity);
    }
}
