using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Sample.Core.Models;
using Sample.Core.Security;
using Sample.Web.ViewModels;

namespace Sample.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> userManager;

        public AccountController(DataContext context)
        {
            _context = context;
            userManager = new UserManager<User>(new UserStore<User>(_context))
            {
                UserValidator = new EmailUsernameIdentityValidator(),
                ClaimsIdentityFactory = new UserClaimsIdentityFactory()
            };
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //// Don't do this in production!
            //if (model.Email == "admin@admin.com" && model.Password == "password")
            //{
            //    var identity = new ClaimsIdentity(new[]
            //    {
            //        new Claim(ClaimTypes.Name, "Ben"),
            //        new Claim(ClaimTypes.Email, "a@b.com"),
            //        new Claim(ClaimTypes.Country, "England")
            //    },
            //        "ApplicationCookie");

            //    IOwinContext ctx = Request.GetOwinContext();
            //    IAuthenticationManager authManager = ctx.Authentication;

            //    authManager.SignIn(identity);

            //    return Redirect(GetRedirectUrl(model.ReturnUrl));
            //}

            var user = await userManager.FindAsync(model.Email, model.Password);

            if (user != null)
            {
                await SignIn(user);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user auth failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        public ActionResult LogOut()
        {
            IOwinContext ctx = Request.GetOwinContext();
            IAuthenticationManager authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("index", "home");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("confirmpassword", "Passwords must match");
                return View();
            }

            var user = new User
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SignIn(user);
                return RedirectToAction("index", "home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        private async Task SignIn(User user)
        {
            var identity = await userManager.CreateIdentityAsync(
                user, DefaultAuthenticationTypes.ApplicationCookie);

            GetAuthenticationManager().SignIn(identity);
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}