namespace Rancho.Services.Identification;

public static class IdentificationConstants
{
    public static string IdentityRoleName => "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    public static string IdentificationQueuePrefix => "_identification";

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
        return @base + IdentificationConstants.IdentificationQueuePrefix;
    }
}
