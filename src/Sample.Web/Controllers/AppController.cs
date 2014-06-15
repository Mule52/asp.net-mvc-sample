using System.Security.Claims;
using System.Web.Mvc;
using Sample.Core.Security;

namespace Sample.Web.Controllers
{
    public abstract class AppController : Controller
    {
        public UserPrincipal CurrentUser
        {
            get { return new UserPrincipal(base.User as ClaimsPrincipal); }
        }
    }
}