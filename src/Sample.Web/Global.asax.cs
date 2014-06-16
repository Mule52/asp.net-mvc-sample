using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sample.Web
{
    public class MvcApplication : HttpApplication
    {
        public static log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(HttpApplication));

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("Sample.Web is starting...");

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.Bootstrap();
        }
    }
}