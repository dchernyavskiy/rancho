using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;

namespace Rancho.Services.Management.Farmers.Features.DeletingFarmer.v1.Events.Integration.External;

public class FarmerDeletedConsumer : IConsumer<FarmerDeletedV1>
{
    private readonly IManagementDbContext _context;

    public FarmerDeletedConsumer(IManagementDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<FarmerDeletedV1> context)
    {
        try
        {
            var entity = await _context.Farmers
                             .FirstOrDefaultAsync(x => x.Id == context.Message.Id);

            Guard.Against.Null(entity, nameof(entity) + " was not found.");

            _context.Farmers.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch
        {

        }
    }
}
