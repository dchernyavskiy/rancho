using Rancho.Services.Identity.Shared.Models;

namespace Rancho.Services.Identity.Users.Features.UpdatingUser.v1;

public record UpdateUserRequest
{
    public UserState UserState { get; init; }
}
