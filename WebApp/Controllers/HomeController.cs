using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BackendApiClient _apiClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(BackendApiClient apiClient, ILogger<HomeController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? name, string? category, string? sortByPrice)
        {
            try
            {
                var items = await _apiClient.GetItemsAsync(name, category, sortByPrice);

                var allItems = await _apiClient.GetItemsAsync();
                var categories = allItems
                    .Select(i => i.Category)
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

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
                _logger.LogError(ex, "Failed to load product catalog.");
                return View(new CatalogViewModel());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
