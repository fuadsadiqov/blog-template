namespace GP.Core.Models
{
    public static class CustomClaimTypes
    {
        internal const string ClaimTypeNamespace = "http://schemas.microsoft.com/ws/2008/06/identity/claims";
        public const string Application = ClaimTypeNamespace + "/application";
        public const string Domain = ClaimTypeNamespace + "/domain";
        public const string CanAccessOutOfDomain = ClaimTypeNamespace + "/canaccessoutoufdomain";
        public const string RememberMe = ClaimTypeNamespace + "/rememberme";
        public const string Impersonator = ClaimTypeNamespace + "/impersonator";
        public const string ImpersonatorName = ClaimTypeNamespace + "/impersonatorName";

        public static List<string> Types = new()
        {
            Application,
            CanAccessOutOfDomain,
            Domain,
            RememberMe,
            Impersonator,
            ImpersonatorName
        };
    }
}
