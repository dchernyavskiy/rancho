using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

namespace Rancho.Services.Management.Farmers.Features.UpdatingFarmer.v1.Events.Integration.External;

public class FarmerUpdatedConsumer : IConsumer<FarmerUpdatedV1>
{
    private readonly IManagementDbContext _context;

    public FarmerUpdatedConsumer(IManagementDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<FarmerUpdatedV1> context)
    {
        try
        {
            var entity = await _context.Farmers
                             .FirstOrDefaultAsync(x => x.Id == context.Message.Id);

            Guard.Against.Null(entity, nameof(entity) + " was not found.");

            _context.Farmers.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch
        {
        }
    }
}
