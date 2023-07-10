using Bogus;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Abstractions.Persistence;
using Fare;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Shared.Management.Animals.Events.v1.Integration;
using IBus = MassTransit.IBus;

namespace Rancho.Services.Management.Animals.Data;

public class AnimalDataSeeder : IDataSeeder
{
    public sealed class AnimalSeedFaker : Faker<Animal>
    {
        public AnimalSeedFaker(ICollection<Guid> farmIds)
        {
            var xeger = new Xeger(@"[A-Z]{2}\d{4}", new Random());
            CustomInstantiator(
                faker => new Animal()
                         {
                             EarTagNumber = xeger.Generate(),
                             BirthDate = faker.Date.Past(2),
                             Gender = faker.PickRandom<Gender>(),
                             FarmId = faker.PickRandom(farmIds),
                             Species = faker.Lorem.Word()
                         });
        }
    }

    private readonly IManagementDbContext _context;
    private readonly IMessagePersistenceService _messagePersistenceService;
    private readonly IBus _bus;

    public AnimalDataSeeder(
        IManagementDbContext context,
        IMessagePersistenceService messagePersistenceService,
        IBus bus
    )
    {
        _context = context;
        _messagePersistenceService = messagePersistenceService;
        _bus = bus;
        Order = 4;
    }

    public async Task SeedAllAsync()
    {
        if (await _context.Animals.AnyAsync()) return;

        var farmIds = await _context.Farms.Select(x => x.Id).ToListAsync();
        var farm =
            await _context.Farms.FirstOrDefaultAsync(
                x => x.Farmers.Select(x => x.Id).Contains(ManagementConstants.FarmerId));

        var animals = new AnimalSeedFaker(farmIds).Generate(20);
        if (farm != null) animals.AddRange(new AnimalSeedFaker(new List<Guid>() {farm.Id}).Generate(20));
        await _context.Animals.AddRangeAsync(animals);
        await _context.SaveChangesAsync();

        foreach (var animal in animals)
        {
            await _messagePersistenceService.AddPublishMessageAsync(
                    new MessageEnvelope<AnimalCreatedV1>(
                        new AnimalCreatedV1(
                            animal.Id,
                            animal.Species,
                            animal.BirthDate,
                            animal.Gender.ToString(),
                            animal.EarTagNumber,
                            animal.FarmId),
                        new Dictionary<string, object?>()),
                    CancellationToken.None)
                .ContinueWith(
                    async _ =>
                    {
                        await _messagePersistenceService.AddPublishMessageAsync(
                            new MessageEnvelope<AnimalCreatedV1>(
                                new AnimalCreatedV1(
                                    animal.Id,
                                    animal.Species,
                                    animal.BirthDate,
                                    animal.Gender.ToString(),
                                    animal.EarTagNumber,
                                    animal.FarmId),
                                new Dictionary<string, object?>()),
                            CancellationToken.None);
                    });
        }
    }

    public int Order { get; }
}
