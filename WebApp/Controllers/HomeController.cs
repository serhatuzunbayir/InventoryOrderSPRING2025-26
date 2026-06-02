using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    // HomeController manages the customer-facing product catalog page.
    public class HomeController : Controller
    {
        private readonly BackendApiClient _apiClient;
        private readonly ILogger<HomeController> _logger;

        // Constructor to inject the API client and logger services.
        public HomeController(BackendApiClient apiClient, ILogger<HomeController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        // Action method for the catalog main page, handling filtering, searching and sorting.
        public async Task<IActionResult> Index(string? name, string? category, string? sortByPrice)
        {
            try
            {
                // Fetch the list of items matching the search, filter, and sort parameters
                var items = await _apiClient.GetItemsAsync(name, category, sortByPrice);

                // Fetch all items to extract the unique categories for the dropdown menu
                var allItems = await _apiClient.GetItemsAsync();
                var categories = allItems
                    .Select(i => i.Category)
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                // Prepare the catalog view model with the list of items and filter values
                var model = new CatalogViewModel
                {
                    Items = items,
                    Categories = categories,
                    Name = name,
                    Category = category,
                    SortByPrice = sortByPrice
                };

                return View(model);
            }
            catch (Exception ex)
            {
                // Log exception details and return an empty catalog on error
                _logger.LogError(ex, "Failed to load product catalog.");
                return View(new CatalogViewModel());
            }
        }

        // Action to display the error page when exceptions occur.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
