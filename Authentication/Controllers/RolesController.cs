using Authentication.Customized;
using Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<CustomUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<CustomUser> userManager)
        {
            this._rolemanager = roleManager;
            this._userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            var roles = _rolemanager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(RoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = model.Name
                };

                var result = await _rolemanager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Redirect("index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }

            return View(model);

        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string Name)
        {

            var Role = await _rolemanager.FindByNameAsync(Name);
            if (Role != null)
            {
                var result = await _rolemanager.DeleteAsync(Role);
                if (result.Succeeded)
                {
                    return Redirect("index");
                }
                else
                {
                    ModelState.AddModelError("", "Error occurred while deleting the role.");
                }
            }
            else
            {
                var error = new ErrorViewModel { ErrorMessage = "Role Not Found" };
                return View("RoleNotFound", error);
            }

            return View();
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UserRole(string id)
        {
            var CurrentUser = await _userManager.FindByIdAsync(id);
            var Roles = _rolemanager.Roles.ToList();
            var userRoles = (await _userManager.GetRolesAsync(CurrentUser)).ToList();

            var model = new UserRoleViewModel
            {
                CurrentUser = CurrentUser,
                Roles = Roles,
                UserRoles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UserRole(string id, List<string> Roles)
        {
            var CurrentUser = await _userManager.FindByIdAsync(id);
            var ListOfRoles = _rolemanager.Roles.ToList();

            if (CurrentUser != null)
            {
                foreach (var item in ListOfRoles)
                {
                    if (Roles.Contains(item.Name))
                    {
                        if (!(await _userManager.IsInRoleAsync(CurrentUser, item.Name)))
                        {
                            await _userManager.AddToRoleAsync(CurrentUser, item.Name);

                        }

                    }
                    else
                    {
                        if ((await _userManager.IsInRoleAsync(CurrentUser, item.Name)))
                        {
                            await _userManager.RemoveFromRoleAsync(CurrentUser, item.Name);
                        }
                    }
                }
            }
            return RedirectToAction("index", "manage");
        }
    }
}
