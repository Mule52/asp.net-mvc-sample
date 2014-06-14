using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using log4net;

namespace Sample.Web
{
    public static class AutofacConfig
    {
        public static void Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.Register(b => LogManager.GetLogger(typeof (HttpApplication)));

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}