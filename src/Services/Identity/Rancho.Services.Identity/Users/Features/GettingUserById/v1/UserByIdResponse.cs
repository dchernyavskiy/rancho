using Rancho.Services.Identity.Users.Dtos;
using Rancho.Services.Identity.Users.Dtos.v1;

namespace Rancho.Services.Identity.Users.Features.GettingUserById.v1;

internal record UserByIdResponse(IdentityUserDto IdentityUser);
