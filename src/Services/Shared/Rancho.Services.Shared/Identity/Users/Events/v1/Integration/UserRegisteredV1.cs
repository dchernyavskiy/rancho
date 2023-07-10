using BuildingBlocks.Core.Messaging;

namespace Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

public record UserRegisteredV1(
    Guid IdentityId,
    string Email,
    string PhoneNumber,
    string UserName,
    string FirstName,
    string LastName,
    List<string>? Roles
) : IntegrationEvent;

public record UserUpdatedV1(
    Guid Id,
    string Email,
    string PhoneNumber,
    string FirstName,
    string LastName
) : IntegrationEvent;
