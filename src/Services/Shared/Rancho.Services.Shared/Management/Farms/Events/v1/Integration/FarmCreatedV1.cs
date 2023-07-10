using BuildingBlocks.Core.Messaging;

namespace Rancho.Services.Shared.Management.Farms.Events.v1.Integration;

public record FarmCreatedV1(Guid Id, string Name, string Address, Guid OwnerId) : IntegrationEvent;

public record FarmUpdatedV1(Guid Id) : IntegrationEvent;

public record FarmDeletedV1(Guid Id) : IntegrationEvent;
