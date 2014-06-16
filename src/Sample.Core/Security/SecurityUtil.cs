using System;
using System.Configuration;

namespace Sample.Core.Security
{
    public static class SecurityUtil
    {
        #region Unused
        public static string PasswordSalt
        {
            get
            {
                return ConfigurationManager.AppSettings["PasswordSalt"];
            }
        }

        public static string SaltPassword(string password, string delimiter = "|")
        {
            if (String.IsNullOrEmpty(PasswordSalt))
                throw new ConfigurationErrorsException("\"PasswordSalt\" is required to be set in the \"appSettings\" element of the configuration file.");
            return PasswordSalt + delimiter + password;
        }
        #endregion
        public static class ClaimTypes
        {
            private static readonly string ClaimsBaseUrl = String.Format("http://{0}/claims/", Models.Env.Domain);
            public readonly static string OrganizationId = ClaimsBaseUrl + "organizationid";
            public readonly static string SubDomain = ClaimsBaseUrl + "subdomain";
            public readonly static string FullDomain = ClaimsBaseUrl + "domain";
            public readonly static string SessionTimeoutMinutes = ClaimsBaseUrl + "sessiontimeoutminutes";
        }
    }
}
