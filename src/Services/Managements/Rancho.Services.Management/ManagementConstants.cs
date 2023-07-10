namespace Rancho.Services.Management;

public static class ManagementConstants
{
    public static string IdentityRoleName => "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    public static readonly Guid FarmerId = Guid.Parse("df44d97a-77b8-44a1-99ce-cf6667491db2");
    public static string ManagementQueuePrefix => "_identification";

    public static class Role
    {
        public const string Admin = "admin";
        public const string User = "user";
        public const string Farmer = "farmer";
    }
}

public static class StringExtensions
{
    internal static string Prefixify(this string @base)
    {
        return @base + ManagementConstants.ManagementQueuePrefix;
    }
}
