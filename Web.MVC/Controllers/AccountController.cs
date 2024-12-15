using Microsoft.AspNetCore.Mvc;
using Web.Application.DTOs.Identity;
using Web.Application.Interfaces;

namespace Web.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var result = await _authService.RegisterUserAsync(registerDto);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerDto); 
            }

            TempData["SuccessMessage"] = "User registered successfully!";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var userId = await _authService.LoginUserAsync(loginDto);
            if (userId == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(loginDto);
            }

            TempData["SuccessMessage"] = "Login successful!";
            return RedirectToAction("Index", "Product"); 
        }

    }
}
