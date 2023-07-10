using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Management.Animals.Enums;
using Rancho.Services.Management.Animals.Models;
using Rancho.Services.Management.Shared.Contracts;
using Rancho.Services.Management.Works.Models;

namespace Rancho.Services.Management.Works.Data;

public class WorkDataSeeder : IDataSeeder
{
    public sealed class WorkSeedFaker : Faker<Work>
    {
        public WorkSeedFaker(ICollection<Guid> farmerIds)
        {
            CustomInstantiator(
                faker => new Work()
                         {
                             Name = faker.Lorem.Word(),
                             Description = faker.Random.Words(25),
                             Start = faker.Date.Past(2),
                             End = faker.Date.Future(2),
                             FarmerId = ManagementConstants.FarmerId
                         });
        }
    }

    private readonly IManagementDbContext _context;

    public WorkDataSeeder(IManagementDbContext context)
    {
        _context = context;
        Order = 3;
    }

    public async Task SeedAllAsync()
    {
        if (await _context.Animals.AnyAsync()) return;

        var farmerIds = await _context.Farmers.Select(x => x.Id).ToListAsync();
        var entities = new WorkSeedFaker(farmerIds).Generate(100);

        await _context.Works.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public int Order { get; }
}
