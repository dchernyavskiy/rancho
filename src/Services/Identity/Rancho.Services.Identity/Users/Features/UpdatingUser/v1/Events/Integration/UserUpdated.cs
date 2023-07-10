using BuildingBlocks.Core.Messaging;
using Rancho.Services.Identity.Shared.Models;

namespace Rancho.Services.Identity.Users.Features.UpdatingUser.v1.Events.Integration;

public record UserUpdated(Guid UserId, UserState OldUserState, UserState NewUserState) : IntegrationEvent;
