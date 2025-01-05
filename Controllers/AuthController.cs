using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MINIBANK.Models;
using MINIBANK.Services.Interface;
using Microsoft.AspNetCore.Authorization;

namespace MINIBANK.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        // Admin Index
        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.Identity.Name;
                ViewBag.ex = userEmail; // Display user email
                return RedirectToAction("GetAllUsers", "User"); // Redirect to user management for admin
            }
            return RedirectToAction("Login"); // Redirect to Login if not authenticated
        }

        // Login GET
        public IActionResult Login() => View();

        // Login POST
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var user = authService.Login(email, password);

                // Authenticate and create cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                // Redirect based on user role
                // Assuming UserRole is an enum like this:
                // public enum UserRole { Admin, User }

                return RedirectToAction(user.Role == UserRole.Admin ? "AdminDashboard" : "UserDashboard", "Auth");
            }
            catch (UnauthorizedAccessException)
            {
                ViewBag.Error = "Invalid credentials. Please try again.";
                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "An error occurred while logging in.";
                return View();
            }
        }

        // Signup GET
        public IActionResult Signup() => View();

        // Signup POST
        [HttpPost]
        public IActionResult Signup(Users user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    authService.Register(user);
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(user);
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // User Dashboard
        [Authorize(Roles = "User")]
        public IActionResult UserDashboard()
        {
            ViewData["Title"] = "User Dashboard";
            return View();
        }

        // Admin Dashboard
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }
    }
}
