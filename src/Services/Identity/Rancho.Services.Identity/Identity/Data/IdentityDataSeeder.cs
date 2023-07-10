using BuildingBlocks.Abstractions.Persistence;
using Microsoft.AspNetCore.Identity;
using Rancho.Services.Identity.Shared.Models;

namespace Rancho.Services.Identity.Identity.Data;

public class IdentityDataSeeder : IDataSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityDataSeeder(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAllAsync()
    {
        await SeedRoles();
        await SeedUsers();
    }

    public int Order => 1;

    private async Task SeedRoles()
    {
        if (!await _roleManager.RoleExistsAsync(ApplicationRole.Admin.Name))
            await _roleManager.CreateAsync(ApplicationRole.Admin);

        if (!await _roleManager.RoleExistsAsync(ApplicationRole.User.Name))
            await _roleManager.CreateAsync(ApplicationRole.User);

        if (!await _roleManager.RoleExistsAsync(ApplicationRole.Farmer.Name))
            await _roleManager.CreateAsync(ApplicationRole.Farmer);
    }

    private async Task SeedUsers()
    {
        if (await _userManager.FindByEmailAsync("mehdi@test.com") == null)
        {
            var user = new ApplicationUser
                       {
                           Id = Guid.Parse("9e0dc2fe-a49d-45be-b3bd-4a3e370dc5c3"),
                           UserName = "mehdi",
                           FirstName = "Mehdi",
                           LastName = "test",
                           Email = "mehdi@test.com",
                       };

            var result = await _userManager.CreateAsync(user, "123456");

            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, ApplicationRole.Admin.Name);
        }

        if (await _userManager.FindByEmailAsync("mehdi2@test.com") == null)
        {
            var user = new ApplicationUser
            {
                Id = Guid.Parse("df44d97a-77b8-44a1-99ce-cf6667491db2"),
                UserName = "mehdi2",
                FirstName = "Mehdi",
                LastName = "Test",
                Email = "mehdi2@test.com"
            };

            var result = await _userManager.CreateAsync(user, "123456");

            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, ApplicationRole.User.Name);
        }
    }
}
