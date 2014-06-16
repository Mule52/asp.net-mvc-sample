using System.Configuration;

namespace Sample.Core.Models
{
    public static class Env
    {
        public static readonly string Domain;

        static Env()
        {
            Domain = GetDomain();
        }

        private static string GetDomain()
        {
            try
            {
                return ConfigurationManager.AppSettings["Domain"];
            }
            catch
            {
                return "SampleWeb";
            }
        }
    }
}
