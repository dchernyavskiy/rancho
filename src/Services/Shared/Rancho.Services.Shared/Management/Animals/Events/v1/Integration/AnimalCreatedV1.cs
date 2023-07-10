using BuildingBlocks.Core.Messaging;

namespace Rancho.Services.Shared.Management.Animals.Events.v1.Integration;

public record AnimalCreatedV1(
    Guid Id,
    string Species,
    DateTime BirthDate,
    string Gender,
    string EarTagNumber,
    Guid FarmId
) : IntegrationEvent;

public record AnimalDeletedV1(Guid Id) : IntegrationEvent;

public record AnimalUpdatedV1(
    Guid Id,
    string Species,
    DateTime BirthDate,
    string Gender,
    string EarTagNumber,
    Guid FarmId
) : IntegrationEvent;
