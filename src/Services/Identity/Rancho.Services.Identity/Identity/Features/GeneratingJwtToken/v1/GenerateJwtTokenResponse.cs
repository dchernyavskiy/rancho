namespace Rancho.Services.Identity.Identity.Features.GeneratingJwtToken.v1;

public record GenerateJwtTokenResponse(string Token, DateTime ExpireAt);
