using Bogus;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rancho.Services.Management.Farms.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Management.Farms.Events.v1.Integration;
using IBus = MassTransit.IBus;

namespace Rancho.Services.Management.Animals.Data;

public class FarmDataSeeder : IDataSeeder
{
    public sealed class FarmSeedFaker : Faker<Farm>
    {
        public FarmSeedFaker()
        {
            CustomInstantiator(
                faker => new Farm()
                         {
                             Name = faker.Company.CompanyName(),
                             Address = faker.Address.FullAddress(),
                             OwnerId = Guid.Parse("9e0dc2fe-a49d-45be-b3bd-4a3e370dc5c3")
                         });
        }
    }

    private readonly IManagementDbContext _context;
    private readonly IMessagePersistenceService _messagePersistenceService;

    public FarmDataSeeder(IManagementDbContext context, IMessagePersistenceService messagePersistenceService)
    {
        _context = context;
        _messagePersistenceService = messagePersistenceService;
        Order = 1;
    }

    public async Task SeedAllAsync()
    {
        if (await _context.Animals.AnyAsync()) return;

        var farms = new FarmSeedFaker().Generate(20);

        await _context.Farms.AddRangeAsync(farms);
        await _context.SaveChangesAsync();

        foreach (var farm in farms)
        {
            await _messagePersistenceService.AddPublishMessageAsync(
                new MessageEnvelope<FarmCreatedV1>(
                    new FarmCreatedV1(farm.Id, farm.Name, farm.Address, farm.OwnerId),
                    new Dictionary<string, object?>()),
                CancellationToken.None);
        }
    }

    public int Order { get; }
}
