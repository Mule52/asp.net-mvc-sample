using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Sample.Web;

[assembly: OwinStartup("LocalConfiguration", typeof (LocalStartup))]

namespace Sample.Web
{
    public class LocalStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/account/login"),
                LogoutPath = new PathString("/account/logoff")
            });
        }
    }
}