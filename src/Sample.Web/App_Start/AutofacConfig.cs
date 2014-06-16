using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Sample.Core.Models;

namespace Sample.Web
{
    public static class AutofacConfig
    {
        public static void Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.Register(b => LogManager.GetLogger(typeof(HttpApplication)));

            builder.Register(ctx => HttpContext.Current.GetOwinContext().Authentication)
                .InstancePerHttpRequest();

            builder.RegisterType<DataContext>().InstancePerHttpRequest().InstancePerApiRequest();
            builder.Register<IUserStore<User>>(ctx => new UserStore<User>(ctx.Resolve<DataContext>()));

            builder.Register<RoleStore<Role>>(ctx => new RoleStore<Role>(ctx.Resolve<DataContext>()));
            builder.Register(x => new RoleManager<Role>(x.Resolve<RoleStore<Role>>()));


            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterFilterProvider();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}