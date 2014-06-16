using System;
using System.Data.Entity.Infrastructure;
using System.Net;
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
        private readonly UserManager<User> _userManager;
        private readonly UserStore<User> _userStore;

        public AccountController(DataContext context)
        {
            _context = context;
            _userStore = new UserStore<User>(_context);
            _userManager = new UserManager<User>(_userStore)
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

            User user = await _userManager.FindAsync(model.Email, model.Password);

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

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SignIn(user);
                return RedirectToAction("index", "home");
            }

            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new UpdateUserModel();
            model.Id = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.UserName = user.UserName;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateUserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _userManager.Find(model.UserName, model.OldPassword);
                    if (user != null)
                    {
                        String userId = User.Identity.GetUserId();
                        String hashedNewPassword = _userManager.PasswordHasher.HashPassword(model.NewPassword);
                        User cUser = await _userStore.FindByIdAsync(userId);
                        await _userStore.SetPasswordHashAsync(cUser, hashedNewPassword);
                        await _userStore.UpdateAsync(cUser);
                        return RedirectToAction("index", "home");
                    }
                    return new HttpStatusCodeResult(400, "Incorrect username or password");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("",
                    "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(model);
        }

        private async Task SignIn(User user)
        {
            ClaimsIdentity identity = await _userManager.CreateIdentityAsync(
                user, DefaultAuthenticationTypes.ApplicationCookie);

            GetAuthenticationManager().SignIn(identity);
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            IOwinContext ctx = Request.GetOwinContext();
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
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}