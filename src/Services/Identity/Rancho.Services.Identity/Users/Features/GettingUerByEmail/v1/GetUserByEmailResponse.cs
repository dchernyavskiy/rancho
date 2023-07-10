using Rancho.Services.Identity.Users.Dtos;
using Rancho.Services.Identity.Users.Dtos.v1;

namespace Rancho.Services.Identity.Users.Features.GettingUerByEmail.v1;

public record GetUserByEmailResponse(IdentityUserDto? UserIdentity);
