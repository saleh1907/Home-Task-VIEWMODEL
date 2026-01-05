using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProniaTwo.Context;
using ProniaTwo.Models;
using ProniaTwo.ViewModels.UserViewModels;
using System.Threading.Tasks;

namespace ProniaTwo.Controllers;

public class AuthController(UserManager<AppUser>_userManager) : Controller
{
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register (RegisterVM vm)
    {
        if(!ModelState.IsValid)
        {
            return View(vm);
        }

        AppUser newUser = new()
        {
            FullName = vm.FirstName + " " + vm.LastName,
            Email = vm.EmailAddress,
            UserName=vm.UserName,
        };
    var result= await _userManager.CreateAsync(newUser,vm.Password);

        if(!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(vm);    
        }
        return Ok("Ok");
    }
}
