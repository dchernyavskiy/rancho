using Rancho.Services.Identity.Shared.Models;

namespace Rancho.Services.Identity.Users.Features.UpdatingUserState.v1;

public record UpdateUserStateRequest
{
    public UserState UserState { get; init; }
}
