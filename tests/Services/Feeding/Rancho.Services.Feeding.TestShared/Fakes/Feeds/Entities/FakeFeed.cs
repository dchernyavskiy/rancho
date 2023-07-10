using Bogus;
using Rancho.Services.Feeding.Feeds.Models;
using Rancho.Services.Feeding.Feeds.ValueObjects;

namespace Rancho.Services.Feeding.TestShared.Fakes.Feeds.Entities;

public class FakeFeed : Faker<Feed>
{
    public FakeFeed()
    {
        CustomInstantiator(
            f =>
            {
                return new Feed()
                       {
                           Name = f.Commerce.ProductName(),
                           Type = f.Commerce.ProductAdjective(),
                           Description = f.Lorem.Paragraph(),
                           Nutrition = new Nutrition()
                                       {
                                           Calories = f.Random.Int(100, 1000),
                                           Carbohydrate = f.Random.Int(100, 1000),
                                           Fat = f.Random.Int(100, 1000),
                                           Protein = f.Random.Int(100, 1000)
                                       },
                           WeightInStock = f.Random.Int(100, 1000)
                       };
            });
    }
}
