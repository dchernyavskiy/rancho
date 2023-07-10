using BuildingBlocks.Core.Messaging;

namespace Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

public record FarmerCreatedV1(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber
) : IntegrationEvent
{
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}

public record FarmerUpdatedV1(Guid Id, string FirstName, string LastName) : IntegrationEvent;

public record FarmerDeletedV1(Guid Id) : IntegrationEvent;
