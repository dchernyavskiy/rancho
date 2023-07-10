using Bogus;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Abstractions.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Farmers.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Identity.Users.Events.v1.Integration;
using IBus = MassTransit.IBus;

namespace Rancho.Services.Management.Farmers.Data;

public class FarmerDataSeeder : IDataSeeder
{
    public sealed class FarmerSeedFaker : Faker<Farmer>
    {
        public FarmerSeedFaker(List<Guid> farmIds)
        {
            CustomInstantiator(
                faker => new Farmer()
                         {
                             FirstName = faker.Name.FirstName(),
                             LastName = faker.Name.LastName(),
                             Email = faker.Internet.Email(),
                             PhoneNumber = faker.Phone.PhoneNumber(),
                             FarmId = faker.PickRandom(farmIds),
                         });
        }
    }

    private readonly IManagementDbContext _context;
    private readonly IMessagePersistenceService _messagePersistenceService;
    private readonly IBus _bus;


    public FarmerDataSeeder(IManagementDbContext context, IMessagePersistenceService messagePersistenceService, IBus bus)
    {
        _context = context;
        _messagePersistenceService = messagePersistenceService;
        _bus = bus;
        Order = 2;
    }

    public async Task SeedAllAsync()
    {
        var faker = new Faker();
        var farmIds = await _context.Farms.Select(x => x.Id).ToListAsync();

        var entities = new FarmerSeedFaker(farmIds).Generate(20);
        await _context.Farmers.AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        foreach (var entity in entities)
        {
            // await _messagePersistenceService.AddPublishMessageAsync(
            //     new MessageEnvelope<FarmerCreatedV1>(
            //         new FarmerCreatedV1(entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.PhoneNumber),
            //         new Dictionary<string, object?>()),
            //     CancellationToken.None);
            await _bus.Publish<FarmerCreatedV1>(
                new FarmerCreatedV1(entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.PhoneNumber));
        }

        try
        {
            var farmer = Farmer.Create(
                ManagementConstants.FarmerId,
                "Mehdi",
                "Test",
                "mehdi2@test.com",
                "+9876543210",
                faker.PickRandom(farmIds));
            await _context.Farmers.AddAsync(farmer);
            await _context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
        }
    }

    public int Order { get; }
}
