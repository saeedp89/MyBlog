using Blog.Frontend.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Frontend.Controllers;

public class AuthController(SignInManager<IdentityUser> signInManager) : Controller
{
    [HttpGet]
    public IActionResult Login(string returnUrl = "")
    {
        if (!string.IsNullOrWhiteSpace(returnUrl))
        {
            ViewBag.ReturnUrl = returnUrl;
        }
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "")
    {
        var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

        return RedirectToAction("Index", "Panel");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

}