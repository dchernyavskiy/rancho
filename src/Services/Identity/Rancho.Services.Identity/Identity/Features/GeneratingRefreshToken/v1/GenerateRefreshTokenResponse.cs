using Rancho.Services.Identity.Identity.Dtos;
using Rancho.Services.Identity.Identity.Dtos.v1;

namespace Rancho.Services.Identity.Identity.Features.GeneratingRefreshToken.v1;

public record GenerateRefreshTokenResponse(RefreshTokenDto RefreshToken);
