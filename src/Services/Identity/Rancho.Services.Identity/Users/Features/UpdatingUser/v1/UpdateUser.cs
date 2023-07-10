using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Core.Exception;
using BuildingBlocks.Email;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Rancho.Services.Identity.Shared.Exceptions;
using Rancho.Services.Identity.Shared.Models;
using Rancho.Services.Identity.Users.Features.UpdatingUser.v1.Events.Integration;
using Rancho.Services.Identity.Users.Features.UpdatingUserState.v1.Events.Integration;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

namespace Rancho.Services.Identity.Users.Features.UpdatingUser.v1;

public record UpdateUser() : ITxUpdateCommand
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}

internal class UpdateUserValidator : AbstractValidator<UpdateUser>
{
    public UpdateUserValidator()
    {
        CascadeMode = CascadeMode.Stop;
    }
}

internal class UpdateUserHandler : ICommandHandler<UpdateUser>
{
    private readonly IBus _bus;
    private readonly ILogger<UpdateUserHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly IEmailSender _emailSender;

    public UpdateUserHandler(
        IBus bus,
        UserManager<ApplicationUser> userManager,
        ILogger<UpdateUserHandler> logger,
        ISecurityContextAccessor securityContextAccessor,
        IEmailSender emailSender
    )
    {
        _bus = bus;
        _logger = logger;
        _securityContextAccessor = securityContextAccessor;
        _emailSender = emailSender;
        _userManager = Guard.Against.Null(userManager, nameof(userManager));
    }

    public async Task<Unit> Handle(UpdateUser request, CancellationToken cancellationToken)
    {
        var userId = _securityContextAccessor.UserId;
        var identityUser = await _userManager.FindByIdAsync(userId);
        Guard.Against.NotFound(identityUser, new IdentityUserNotFoundException(userId));

        identityUser!.FirstName = request.FirstName;
        identityUser!.LastName = request.LastName;
        identityUser!.Email = request.Email;
        identityUser!.UserName = request.Email;
        identityUser!.PhoneNumber = request.PhoneNumber;

        await _userManager.UpdateAsync(identityUser);
        var userUpdated = new UserUpdatedV1(
            identityUser.Id,
            identityUser.Email,
            identityUser.PhoneNumber,
            identityUser.FirstName,
            identityUser.LastName);

        var email = new EmailObject(request.Email, "Email was changed", "Email was changed");
        await _emailSender.SendAsync(email);

        await _bus.PublishAsync(userUpdated, null, cancellationToken);

        return Unit.Value;
    }
}
