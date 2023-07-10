using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Identification.Shared.Contracts;
using Rancho.Services.Identification.Animals.Enums;
using Rancho.Services.Identification.Animals.Models;
using Rancho.Services.Identification.Tags.Models;

namespace Rancho.Services.Identification.Animals.Data;

// public class AnimalDataSeeder : IDataSeeder
// {
//     public sealed class AnimalSeedFaker : Faker<Animal>
//     {
//         public AnimalSeedFaker(ICollection<RfidTag> tags)
//         {
//             CustomInstantiator(
//                 faker => Animal.Create(
//                     Guid.NewGuid(),
//                     faker.Name.FirstName(),
//                     faker.Date.Past(2),
//                     faker.PickRandom<Gender>(),
//                     faker.Lorem.Word(),
//                     faker.PickRandom(tags).Id));
//         }
//     }
//
//     private readonly IIdentificationDbContext _context;
//
//
//     public AnimalDataSeeder(IIdentificationDbContext context)
//     {
//         _context = context;
//         Order = 2;
//     }
//
//     public async Task SeedAllAsync()
//     {
//         if (await _context.Animals.AnyAsync() ||
//             !await _context.RfidTags.AnyAsync())
//             return;
//
//         var tags = await _context.RfidTags.ToListAsync();
//         var animals = new AnimalSeedFaker(tags).Generate(20);
//
//         await _context.Animals.AddRangeAsync(animals);
//         await _context.SaveChangesAsync();
//     }
//
//     public int Order { get; }
// }
