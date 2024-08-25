using Authentication.Customized;
using Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Authentication.Controllers
{
    public class ManageController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        public ManageController(UserManager<CustomUser> userManager)
        {
            this._userManager = userManager;
        }

        

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CustomUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Phone = model.Phone,
                    PasswordHash = model.Password,
                    Address = model.Address,
                    FullName = model.FullName,
                    Age = model.Age

                };  

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var ageClaim = new Claim("Age", model.Age.ToString());

                    await _userManager.AddClaimAsync(user,ageClaim);

                    return RedirectToAction("Index");
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



        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var CurrentUser = await _userManager.FindByIdAsync(id);
              
            var info = new UpdateUserVIewModel
            {
                CurrentUser = CurrentUser
            };
            return View(info);

        }



        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserVIewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var userExist = await _userManager.FindByIdAsync(model.CurrentUser.Id);

                if (userExist != null)
                {
                    userExist.FullName = model.CurrentUser.FullName;
                    userExist.Address = model.CurrentUser.Address;
                    userExist.Phone = model.CurrentUser.Phone;
                   
                    var result = await _userManager.UpdateAsync(userExist);
                    if (result.Succeeded )
                    {
                        return RedirectToAction("index");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                else
                {
                    var error = new ErrorViewModel { ErrorMessage = "User Not Found" };
                    return View("UserNotFound", error);
                }

            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ChangePass(string id)
        {
            
                var CurrentUser = await _userManager.FindByIdAsync(id);
                var NewPass = new ChangingPassViewModel
                {
                    CurrentUser = CurrentUser,
                    NewPassword = string.Empty,
                    CurrentPassword = string.Empty
                };
                return View(NewPass);

            
        }


        [HttpPost]
        public async Task<IActionResult> ChangePass(ChangingPassViewModel model )
        {
            if (ModelState.IsValid)
            {
                var CurrentUser = await _userManager.FindByIdAsync(model.CurrentUser.Id);
                if (CurrentUser != null)
                {
                    var result = await _userManager.ChangePasswordAsync(CurrentUser, model.CurrentPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("index");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        {

                        }
                    }

                }
                return View(model);
            }
            return View(model);

        }





        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var userExist = await _userManager.FindByIdAsync(id);


                if (userExist != null)
                {
                    var result = await _userManager.DeleteAsync(userExist);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error occurred while deleting the user.");
                    }

                }
                else
                {
                    var error = new ErrorViewModel { ErrorMessage = "User Not Found" };
                    return View("UserNotFound", error);
                }
            }
            return RedirectToAction("Index");

        }

        [Authorize(Policy ="Age")]
        public IActionResult UserInfo()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

    }
}
