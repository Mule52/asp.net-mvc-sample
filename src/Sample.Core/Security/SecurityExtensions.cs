using System.Linq;
using Sample.Core.Models;

namespace Sample.Core.Security
{
    public static class SecurityExtensions
    {
        public static bool HasClaim(this User user, string claimType, params string[] claimValues)
        {
            return user.Claims.Any(x => x.ClaimType == claimType && claimValues.Contains(x.ClaimValue));
        }
    }
}