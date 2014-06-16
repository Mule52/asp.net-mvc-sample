using System.Configuration;

namespace Sample.Web.Test.Automation.Common
{
    public static class Url
    {
        public static string DomainHost = ConfigurationManager.AppSettings["Domain"];
        public static string DomainPrepath = @"http://" + DomainHost;
        public static string TestDomain = "sample.com";
    }
}
