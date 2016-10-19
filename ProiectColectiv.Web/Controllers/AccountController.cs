using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectColectiv.Core.Constants;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Web.ViewModel;

namespace ProiectColectiv.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        #region Login

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
            if (result.Succeeded)
                return RedirectToLocal(returnUrl);

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }

        #endregion

        #region Register

        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User { UserName = model.Username, Email = model.Email };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.CITITOR);

                return RedirectToAction(nameof(AccountController.Login), "Account");
            }

            AddErrors(result);

            return View(model);
        }

        #endregion

        #region Log Off

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        #endregion

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}