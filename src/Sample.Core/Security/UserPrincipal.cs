using System.Security.Claims;

namespace Sample.Core.Security
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public UserPrincipal(ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public string Name
        {
            get
            {
                return FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string Country
        {
            get
            {
                return FindFirst(ClaimTypes.Country).Value;
            }
        }
    }
}
