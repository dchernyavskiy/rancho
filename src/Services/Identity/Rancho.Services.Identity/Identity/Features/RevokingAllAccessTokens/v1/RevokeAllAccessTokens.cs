using BuildingBlocks.Abstractions.CQRS.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Rancho.Services.Identity.Identity.Features.RevokingAccessToken.v1;
using Rancho.Services.Identity.Shared.Exceptions;
using Rancho.Services.Identity.Shared.Models;

namespace Rancho.Services.Identity.Identity.Features.RevokingAllAccessTokens.v1;

public record RevokeAllAccessTokens(string UserName) : ICommand;

public class RevokeAllAccessTokenHandler : ICommandHandler<RevokeAllAccessTokens>
{
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentityDbContext _identityDbContext;

    public RevokeAllAccessTokenHandler(
        IdentityDbContext identityDbContext,
        IMediator mediator,
        UserManager<ApplicationUser> userManager
    )
    {
        _identityDbContext = identityDbContext;
        _mediator = mediator;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(RevokeAllAccessTokens request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByNameAsync(request.UserName);
        if (appUser == null)
        {
            throw new IdentityUserNotFoundException(request.UserName);
        }

        var tokens = _identityDbContext
            .Set<AccessToken>()
            .Where(x => x.UserId == appUser.Id && x.ExpiredAt > DateTime.Now);

        foreach (var accessToken in tokens)
        {
            await _mediator.Send(new RevokeAccessToken(accessToken.Token, appUser.UserName), cancellationToken);
        }

        return Unit.Value;
    }
}
