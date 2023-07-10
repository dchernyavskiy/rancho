namespace Rancho.Services.Feeding;

public static class FeedingConstants
{
    public static string IdentityRoleName => "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    public static string FeedingQueuePrefix => "_feeding";
    public static class Role
    {
        public const string Admin = "admin";
        public const string User = "user";
    }
}

public static class StringExtensions
{
    internal static string Prefixify(this string @base)
    {
        return @base + FeedingConstants.FeedingQueuePrefix;
    }
}
