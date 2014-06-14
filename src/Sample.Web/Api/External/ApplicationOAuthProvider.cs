using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Sample.Core.Models;
using Sample.Core.Security;

namespace Sample.Web.Api.External
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly Func<UserManager<User>> _userManagerFactory;

        public ApplicationOAuthProvider(string publicClientId, Func<UserManager<User>> userManagerFactory)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            if (userManagerFactory == null)
            {
                throw new ArgumentNullException("userManagerFactory");
            }

            _publicClientId = publicClientId;
            _userManagerFactory = userManagerFactory;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var userManager = _userManagerFactory())
            {
                var user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var oAuthIdentity = await userManager.CreateIdentityAsync(user, context.Options.AuthenticationType);
                var properties = CreateProperties(oAuthIdentity);
                var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                var expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(ClaimsIdentity identity)
        {
            var currentRoles = identity.FindAll(ClaimTypes.Role).Select(cr => cr.Value).ToList();
            var api = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions;

            var allowedActions = api.Where(x =>
            {
                var requiredRoles = GetRequiredRolesForAction(x);
                return (requiredRoles.Any()) ? requiredRoles.Intersect(currentRoles).Any() : true;
            });

            var endpointsByController = allowedActions
                .GroupBy(a => a.ActionDescriptor.ControllerDescriptor.ControllerName, d => d.HttpMethod.Method + " " + d.RelativePath)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.ToArray());

            var data = new Dictionary<string, string>();
            data.Add("userName", identity.Name);
            data.Add("domain", identity.FindFirstValue(SecurityUtil.ClaimTypes.FullDomain));
            data.Add("api", JsonConvert.SerializeObject(endpointsByController, Formatting.Indented));

            return new AuthenticationProperties(data);
        }

        private static IEnumerable<string> GetRequiredRolesForAction(ApiDescription x)
        {
            //The last authorized attribute wins the race
            var actionAuthAttribute = x.ActionDescriptor.GetCustomAttributes<AuthorizeAttribute>().LastOrDefault();
            var controllerAuthAttribute = x.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AuthorizeAttribute>().LastOrDefault();

            var authAttribute = actionAuthAttribute ?? controllerAuthAttribute ?? new AuthorizeAttribute();
            return authAttribute.Roles.Split(',').Select(r => r.Trim());
        }
    }
}