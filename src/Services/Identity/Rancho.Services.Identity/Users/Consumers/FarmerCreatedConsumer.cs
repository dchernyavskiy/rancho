using System.Text;
using Bogus;
using Elasticsearch.Net.Specification.CatApi;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Rancho.Services.Identity.Shared.Models;
using Rancho.Services.Identity.Users.Features.RegisteringUser.v1;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

namespace Rancho.Services.Identity.Users.Consumers;

public class FarmerCreatedConsumer : IConsumer<FarmerCreatedV1>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<FarmerCreatedConsumer> _logger;

    public FarmerCreatedConsumer(UserManager<ApplicationUser> userManager, ILogger<FarmerCreatedConsumer> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<FarmerCreatedV1> context)
    {
        try
        {
            var request = context.Message;
            var applicationUser = await _userManager.FindByEmailAsync(request.Email);

            if (applicationUser is not null) return;

            applicationUser = new ApplicationUser
                              {
                                  Id = request.Id,
                                  FirstName = request.FirstName,
                                  LastName = request.LastName,
                                  UserName = request.Email,
                                  Email = request.Email,
                                  PhoneNumber = request.PhoneNumber,
                                  UserState = UserState.Active,
                                  CreatedAt = request.CreatedAt,
                              };

            var password = GeneratePassword();
            var identityResult = await _userManager.CreateAsync(applicationUser, password);
            if (!identityResult.Succeeded)
                throw new RegisterIdentityUserException(
                    string.Join(',', identityResult.Errors.Select(e => e.Description)));

            var roleResult = await _userManager.AddToRolesAsync(
                                 applicationUser,
                                 new List<string> {IdentityConstants.Role.Farmer});

            if (!roleResult.Succeeded)
                throw new RegisterIdentityUserException(string.Join(',', roleResult.Errors.Select(e => e.Description)));

            _logger.LogInformation(
                $"Farmer {
                    request.FirstName
                } {
                    request.LastName
                } created with email {
                    request.Email
                } and password {
                    password
                }");
        }
        catch
        {
        }
    }

    public static string GeneratePassword(int length = 10)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var random = new Random();

        // Generate a random password using the valid characters
        var passwordBuilder = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            int randomIndex = random.Next(validChars.Length);
            passwordBuilder.Append(validChars[randomIndex]);
        }

        return passwordBuilder.ToString();
    }
}
