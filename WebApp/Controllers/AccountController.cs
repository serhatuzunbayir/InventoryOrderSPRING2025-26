using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly BackendApiClient _apiClient;
        private readonly ILogger<AccountController> _logger;

        public AccountController(BackendApiClient apiClient, ILogger<AccountController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _apiClient.LoginAsync(model);
            if (!result.Success || result.Data == null)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Invalid username or password.");
                return View(model);
            }

            if (result.Data.UserType != "Customer")
            {
                ModelState.AddModelError("", "Access restricted. Only Customers can access the web application.");
                return View(model);
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("jwt_token", result.Data.Token, cookieOptions);
            Response.Cookies.Append("username", result.Data.Username, new CookieOptions { Expires = DateTime.UtcNow.AddDays(7) });

            TempData["SuccessMessage"] = $"Welcome back, {result.Data.Username}!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            model.UserType = "Customer";
            var result = await _apiClient.RegisterAsync(model);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Registration failed.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Registration successful! Please log in.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            Response.Cookies.Delete("username");
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
                return RedirectToAction(nameof(Login), new { returnUrl = Url.Action("Profile", "Account") });

            var profile = await _apiClient.GetProfileAsync();
            if (profile == null)
            {
                TempData["ErrorMessage"] = "Session expired or invalid. Please log in again.";
                return RedirectToAction(nameof(Login));
            }

            var addresses = await _apiClient.GetAddressesAsync();
            ViewBag.Addresses = addresses;

            var model = new UpdateUserRequest
            {
                Email = profile.Email,
                PhoneNumber = profile.PhoneNumber,
                FirstName = profile.FirstName,
                LastName = profile.LastName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateUserRequest model)
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
                return RedirectToAction(nameof(Login));

            if (!ModelState.IsValid)
            {
                var addresses = await _apiClient.GetAddressesAsync();
                ViewBag.Addresses = addresses;
                return View("Profile", model);
            }

            var success = await _apiClient.UpdateProfileAsync(model);
            if (!success)
            {
                TempData["ErrorMessage"] = "Failed to update profile details.";
            }
            else
            {
                TempData["SuccessMessage"] = "Profile details updated successfully!";
            }

            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(AddressRequest model)
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
                return RedirectToAction(nameof(Login));

            if (string.IsNullOrWhiteSpace(model.AddressName) || string.IsNullOrWhiteSpace(model.AddressLine))
            {
                TempData["ErrorMessage"] = "Address details cannot be empty.";
                return RedirectToAction(nameof(Profile));
            }

            var address = await _apiClient.AddAddressAsync(model);
            if (address == null)
            {
                TempData["ErrorMessage"] = "Failed to create address.";
            }
            else
            {
                TempData["SuccessMessage"] = $"Address '{model.AddressName}' added successfully!";
            }

            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
                return RedirectToAction(nameof(Login));

            var success = await _apiClient.DeleteAddressAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Address deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete address.";
            }

            return RedirectToAction(nameof(Profile));
        }
    }
}
