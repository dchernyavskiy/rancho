namespace Rancho.Services.Identification;

public static class IdentificationCacheKey
{
    public static string ProductsByCategory(long categoryId) => $"{nameof(ProductsByCategory)}{categoryId}";

    public static string ProductsWithDiscounts(long id) => $"{nameof(ProductsWithDiscounts)}{id}";
}
