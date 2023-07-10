using Bogus;
using Rancho.Services.Feeding.Feeds.Features.RegisteringFeed.v1;
using Rancho.Services.Feeding.Feeds.ValueObjects;

namespace Rancho.Services.Feeding.TestShared.Fakes.Feeds.Features;

public class FakeRegisterFeed : Faker<RegisterFeed>
{
    public FakeRegisterFeed()
    {
        CustomInstantiator(
            f =>
            {
                return new RegisterFeed()
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
