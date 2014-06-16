using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Sample.Core.Models;

namespace Sample.Core.Security
{
    public class UserClaimsIdentityFactory : ClaimsIdentityFactory<User>
    {
        public override Task<ClaimsIdentity> CreateAsync(UserManager<User, string> manager, User user, string authenticationType)
        {
            return base.CreateAsync(manager, user, authenticationType).ContinueWith(x =>
            {
                var identity = x.Result;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.UserName),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.GivenName, String.Format("{0} {1}", user.FirstName, user.LastName))
                };

                var org = user.Organization;
                var defaultSessionTimeoutFromConfig = ConfigurationManager.AppSettings["DefaultSessionTimeoutMinutes"];

                var defaultSessionTimeout = defaultSessionTimeoutFromConfig == null ? 10 : Convert.ToInt32(defaultSessionTimeoutFromConfig);
                var timeout = (org == null) || (org.SessionTimeoutMinutes < 1) ? defaultSessionTimeout : org.SessionTimeoutMinutes;

                claims.Add(new Claim(SecurityUtil.ClaimTypes.SessionTimeoutMinutes, timeout.ToString(CultureInfo.InvariantCulture)));

                // Add roles
                var roles = manager.GetRoles(user.Id);
                roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));

                return new ClaimsIdentity(identity, claims);
            });
        }
    }
}
