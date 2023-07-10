using BuildingBlocks.Core.Domain;

namespace Rancho.Services.Feeding.Feeds.ValueObjects;

public class Nutrition : ValueObject
{
    public decimal Protein { get; set; }
    public decimal Fat { get; set; }
    public decimal Carbohydrate { get; set; }
    public decimal Calories { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Protein;
        yield return Fat;
        yield return Carbohydrate;
        yield return Calories;
    }
}
