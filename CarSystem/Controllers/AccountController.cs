using CarSystem.Models;
using CarSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
          _userManager = userManager;
           _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Register(RegisterUserViewModel registerVm) 
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { FullName=registerVm.FullName,Address = registerVm.Address, Email = registerVm.Email,PhoneNumber=registerVm.PhoneNumber, UserName = registerVm.Email, UserTypeId = 3 };//Customer By Default
                IdentityResult result = await _userManager.CreateAsync(user, registerVm.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index","Customer");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(registerVm);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserVm)
        {
            if (ModelState.IsValid)
            {
               
                AppUser user = await _userManager.FindByEmailAsync(loginUserVm.Email);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, loginUserVm.Password);
                    if (found)
                    {
                        await _signInManager.SignInAsync(user, loginUserVm.RememberMe);
                        if(user.UserTypeId==3)
                             return RedirectToAction("Index","Customer");
                        if (user.UserTypeId == 2)
                            return RedirectToAction("Index", "Car");
                        if (user.UserTypeId == 1)
                            return RedirectToAction("Index", "Admin");
                    }
                }
                ModelState.AddModelError("", "InValid Login Attempt");
            }
            return View(loginUserVm);
        }
       
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
