using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    // AccountController handles authentication, customer profile management, and address operations.
    public class AccountController : Controller
    {
        private readonly BackendApiClient _apiClient;
        private readonly ILogger<AccountController> _logger;

        // Constructor to inject the backend API communication client and logging services.
        public AccountController(BackendApiClient apiClient, ILogger<AccountController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        // GET: Displays the login view page.
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Validates login credentials and saves the JWT auth token inside cookies.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            // Attempt backend authentication
            var result = await _apiClient.LoginAsync(model);
            if (!result.Success || result.Data == null)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Invalid username or password.");
                return View(model);
            }

            // Restrict login to Customer user types only for this Web App client
            if (result.Data.UserType != "Customer")
            {
                ModelState.AddModelError("", "Access restricted. Only Customers can access the web application.");
                return View(model);
            }

            // Configure cookie options for storing the JWT token securely
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("jwt_token", result.Data.Token, cookieOptions);
            Response.Cookies.Append("username", result.Data.Username, new CookieOptions { Expires = DateTime.UtcNow.AddDays(7) });

            TempData["SuccessMessage"] = $"Welcome back, {result.Data.Username}!";

            // Redirect back to initial page route if local URL exists
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // GET: Displays the registration form page.
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Processes customer registration requests.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid) return View(model);

            // Force UserType to Customer for safety on frontend signups
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

        // POST: Logs out the user by deleting authentication cookies.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            Response.Cookies.Delete("username");
            TempData["SuccessMessage"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }

        // GET: Retrieves and displays user profile and registered shipping addresses.
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Verify active login token exists
            if (!Request.Cookies.ContainsKey("jwt_token"))
                return RedirectToAction(nameof(Login), new { returnUrl = Url.Action("Profile", "Account") });

            // Fetch user profile data
            var profile = await _apiClient.GetProfileAsync();
            if (profile == null)
            {
                TempData["ErrorMessage"] = "Session expired or invalid. Please log in again.";
                return RedirectToAction(nameof(Login));
            }

            // Fetch list of shipping addresses for the template
            var addresses = await _apiClient.GetAddressesAsync();
            ViewBag.Addresses = addresses;

            // Map DTO fields to the update request model
            var model = new UpdateUserRequest
            {
                Email = profile.Email,
                PhoneNumber = profile.PhoneNumber,
                FirstName = profile.FirstName,
                LastName = profile.LastName
            };

            return View(model);
        }

        // POST: Updates personal contact details of the profile.
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

            // Trigger backend profile update API
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

        // POST: Adds a new shipping address to the user's account.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(AddressRequest model)
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
                return RedirectToAction(nameof(Login));

            // Validate that required fields are filled out
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

        // POST: Deletes a specific shipping address by ID.
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
