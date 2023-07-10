using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

namespace Rancho.Services.Management.Farmers.Features.CreatingFarmer.v1.Events.Integration.External;

public class FarmerCreatedConsumer : IConsumer<FarmerCreatedV1>
{
    private readonly IManagementDbContext _context;

    public FarmerCreatedConsumer(IManagementDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<FarmerCreatedV1> context)
    {
        // var farm = (await _context.Farmers
        //                 .Include(x => x.Farm)
        //                 .FirstOrDefaultAsync(x => x.Id == context.Message.CreatedBy))?.Farm;
        //
        // Guard.Against.Null(farm, $"User is not registered on farm");
        //
        // var farmer = Farmer.Create(
        //     context.Message.Id,
        //     context.Message.FirstName,
        //     context.Message.LastName,
        //     farm.Id);
        //
        // await _context.Farmers.AddAsync(farmer);
        await _context.SaveChangesAsync();
    }
}
