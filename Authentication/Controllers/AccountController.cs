using Authentication.Customized;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<CustomUser> _signInManager;

        public AccountController(SignInManager<CustomUser> signInManager)
        {
            this._signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            var url = new LoginViewModel { ReturnUrl = returnUrl };
            return View(url);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    return Redirect(model.ReturnUrl ?? "/");
                 
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("LockedOut", "Your account is locked out due to too many failed login attempts. Please try again later.");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");

        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
