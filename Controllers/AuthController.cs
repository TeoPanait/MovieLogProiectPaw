using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieLog.Models;
using MovieLog.ViewModels;

namespace MovieLog.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    // 1- inregistrare
    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser { 
            UserName = model.Email, 
            Email = model.Email, 
            FullName = model.FullName };
        //incercam sa il salvam cu baza de date cu parola
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User"); // automat rol de user
            await _signInManager.SignInAsync(user, isPersistent: false); //logare instanta
            return RedirectToAction("Index", "Home"); // trimitem pe pag principala
        }
        // daca nu a mers crearea in baza de date, punem erorile pe formular
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model); // returnam erorille
    }
    // 2- logare
    [HttpGet]
    public IActionResult Login(string? returnUrl=null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl=null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        //cautam userul dupa email
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) { 
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
        //verificam parola
        var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure:false);

        if (result.Succeeded) {
            //daca logarea a functionat trimitem unde voia sa ajunga initial sau pe pag home
            if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
    // 3- logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
