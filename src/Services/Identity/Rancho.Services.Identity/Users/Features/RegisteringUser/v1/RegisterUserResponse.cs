using Rancho.Services.Identity.Users.Dtos;
using Rancho.Services.Identity.Users.Dtos.v1;

namespace Rancho.Services.Identity.Users.Features.RegisteringUser.v1;

internal record RegisterUserResponse(IdentityUserDto? UserIdentity);
