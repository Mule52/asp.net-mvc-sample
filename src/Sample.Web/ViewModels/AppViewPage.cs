using System.Security.Claims;
using System.Web.Mvc;
using Sample.Core.Security;

namespace Sample.Web.ViewModels
{
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        protected UserPrincipal CurrentUser
        {
            get
            {
                return new UserPrincipal(this.User as ClaimsPrincipal);
            }
        }
    }

    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }
}