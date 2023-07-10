using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Rancho.Services.Feeding.Shared.Contracts;
using Rancho.Services.Feeding.Animals.Enums;
using Rancho.Services.Feeding.Animals.Models;

namespace Rancho.Services.Feeding.Animals.Data;

// public class AnimalDataSeeder : IDataSeeder
// {
//     public sealed class AnimalSeedFaker : Faker<Animal>
//     {
//         public AnimalSeedFaker()
//         {
//             CustomInstantiator(
//                 faker => new Animal(){
//                     faker.Name.FirstName(),
//                     faker.Lorem.Word(),
//                     faker.Date.Past(2),
//                     faker.PickRandom<Gender>()});
//         }
//     }
//
//     private readonly IFeedingDbContext _context;
//
//
//     public AnimalDataSeeder(IFeedingDbContext context)
//     {
//         _context = context;
//         Order = 2;
//     }
//
//     public async Task SeedAllAsync()
//     {
//         if (await _context.Animals.AnyAsync())
//             return;
//
//         var animals = new AnimalSeedFaker().Generate(20);
//
//         await _context.Animals.AddRangeAsync(animals);
//         await _context.SaveChangesAsync();
//     }
//
//     public int Order { get; }
// }
