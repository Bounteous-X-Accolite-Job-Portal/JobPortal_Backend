using System.Security.Claims;

namespace Bountous_X_Accolite_Job_Portal.Helpers
{
    public static class GetGuidFromString
    {
        public static Guid Get(String? s)
        {
            if (string.IsNullOrWhiteSpace(s) || s.Length != 36)
            {
                return Guid.Empty;
            }

            return new Guid(s);
        }
    }
}
