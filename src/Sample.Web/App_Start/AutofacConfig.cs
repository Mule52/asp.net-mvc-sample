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

            //builder.Register(x =>
            //{
            //    var userManager = new UserManager<User>(x.Resolve<IUserStore<User>>())
            //    {
            //        ClaimsIdentityFactory = new UserClaimsIdentityFactory(),
            //        UserValidator = new EmailUsernameIdentityValidator(),
            //        PasswordValidator = new StrongPasswordIdentityValidator(),
            //        UserLockoutEnabledByDefault = bool.Parse(
            //            ConfigurationManager.AppSettings["AccountLockoutEnabledByDefault"]),
            //        DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(
            //            int.Parse(ConfigurationManager.AppSettings["AccountLockoutMinutes"])),
            //        MaxFailedAccessAttemptsBeforeLockout = int.Parse(
            //            ConfigurationManager.AppSettings["AccountLockoutAttempts"])
            //    };

            //    userManager.RegisterTwoFactorProvider("text",
            //        new TextPhoneNumberTokenProvider(x.Resolve<TwoFactorService>()));

            //    userManager.RegisterTwoFactorProvider("call",
            //        new VoicePhoneNumberTokenProvider(x.Resolve<TwoFactorService>()));

            //    return userManager;
            //});

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