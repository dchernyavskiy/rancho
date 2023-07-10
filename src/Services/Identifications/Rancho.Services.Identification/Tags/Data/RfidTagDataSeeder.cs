using System.Text;
using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Shared.Contracts;
using Rancho.Services.Identification.Tags.Enums;
using Rancho.Services.Identification.Tags.Models;

namespace Rancho.Services.Identification.Tags.Data;

public class RfidTagDataSeeder : IDataSeeder
{
    public static class RfidTagIdGenerator
    {
        public static string GenerateRfidCode(int length)
        {
            const string allowedChars = "0123456789ABCDEF";
            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(allowedChars[random.Next(allowedChars.Length)]);
            }

            return result.ToString();
        }
    }

    public sealed class RfidTagSeedFaker : Faker<RfidTag>
    {
        public RfidTagSeedFaker()
        {
            CustomInstantiator(faker => new RfidTag()
                                        {
                                            Status = faker.PickRandom<Status>(),
                                        });
        }
    }

    private readonly IIdentificationDbContext _context;

    public RfidTagDataSeeder(IIdentificationDbContext context)
    {
        _context = context;
        Order = 1;
    }

    public async Task SeedAllAsync()
    {
        if (await _context.RfidTags.AnyAsync()) return;

        var rfidTags = new RfidTagSeedFaker().Generate(20);

        await _context.RfidTags.AddRangeAsync(rfidTags);
        await _context.SaveChangesAsync();
    }

    public int Order { get; }
}
