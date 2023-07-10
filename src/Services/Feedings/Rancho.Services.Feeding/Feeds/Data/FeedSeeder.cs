using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Feeds.ValueObjects;
using Rancho.Services.Feeding.Shared.Contracts;

namespace Rancho.Services.Feeding.Feeds.Data;

public class FeedSeeder : IDataSeeder
{
    public sealed class FeedSeedFaker : Faker<Feed>
    {
        public FeedSeedFaker(Guid farmId)
        {
            CustomInstantiator(
                f =>
                {
                    return new Feed()
                           {
                               Name = f.PickRandom("hay", "silage", "pasture grass", "corn", "soybean", "cottonseed"),
                               Type = f.PickRandom("Dry", "Wet", "Minerals"),
                               Description = f.Lorem.Text(),
                               Nutrition = new Nutrition()
                                           {
                                               Carbohydrate = f.Random.Int(100, 400),
                                               Fat = f.Random.Int(100, 400),
                                               Protein = f.Random.Int(100, 400),
                                               Calories = f.Random.Int(1000, 4000),
                                           },
                               WeightInStock = f.Random.Int(1000, 10000),
                               FarmId = farmId
                           };
                });
        }
    }

    private readonly IFeedingDbContext _context;
    private readonly ILogger<FeedSeeder> _logger;

    public FeedSeeder(IFeedingDbContext context, ILogger<FeedSeeder> logger)
    {
        _context = context;
        _logger = logger;
        Order = 1;
    }

    public async Task SeedAllAsync()
    {
        var faker = new Faker();

        if (await _context.Farms.AnyAsync())
        {
            var farms = await _context.Farms.Select(x => x.Id).ToListAsync();
            foreach (var id in farms)
            {
                await _context.Feeds.AddRangeAsync(new FeedSeedFaker(id).Generate(faker.Random.Int(10, 20)));
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Feeds was seeded successfully.");
        }
    }

    public int Order { get; }
}
