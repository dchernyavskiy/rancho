using BuildingBlocks.Core.Messaging;
using Rancho.Services.Identity.Shared.Models;

namespace Rancho.Services.Identity.Users.Features.UpdatingUserState.v1.Events.Integration;

public record UserStateUpdated(Guid UserId, UserState OldUserState, UserState NewUserState) : IntegrationEvent;
