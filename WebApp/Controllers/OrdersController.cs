using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly BackendApiClient _apiClient;
        private readonly ILogger<OrdersController> _logger;
        private const string CartCookieName = "shopping_cart";

        public OrdersController(BackendApiClient apiClient, ILogger<OrdersController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        private List<CartItem> GetCart()
        {
            if (Request.Cookies.TryGetValue(CartCookieName, out var json) && !string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
                }
                catch
                {
                    return new List<CartItem>();
                }
            }
            return new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            Response.Cookies.Append(CartCookieName, json, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(14)
            });
        }

        [HttpGet]
        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int itemId, int quantity = 1)
        {
            var item = await _apiClient.GetItemByIdAsync(itemId);
            if (item == null)
            {
                TempData["ErrorMessage"] = "Item not found.";
                return RedirectToAction("Index", "Home");
            }

            if (item.Quantity < quantity)
            {
                TempData["ErrorMessage"] = $"Cannot add {quantity} of '{item.Name}'. Only {item.Quantity} in stock.";
                return RedirectToAction("Index", "Home");
            }

            var cart = GetCart();
            var existing = cart.FirstOrDefault(i => i.ItemId == itemId);
            if (existing != null)
            {
                if (item.Quantity < existing.Quantity + quantity)
                {
                    TempData["ErrorMessage"] = $"Cannot add more. Total cart quantity would exceed stock limit of {item.Quantity}.";
                    return RedirectToAction("Index", "Home");
                }
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Category = item.Category,
                    Quantity = quantity
                });
            }

            SaveCart(cart);
            TempData["SuccessMessage"] = $"Added '{item.Name}' to your shopping cart!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int itemId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                TempData["SuccessMessage"] = $"Removed '{item.Name}' from cart.";
            }
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            Response.Cookies.Delete(CartCookieName);
            TempData["SuccessMessage"] = "Cart cleared.";
            return RedirectToAction(nameof(Cart));
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
            {
                TempData["ErrorMessage"] = "Please sign in to place an order.";
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", "Orders") });
            }

            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction(nameof(Cart));
            }

            var addresses = await _apiClient.GetAddressesAsync();
            if (addresses.Count == 0)
            {
                TempData["ErrorMessage"] = "Please add at least one shipping address in your profile before checking out.";
                return RedirectToAction("Profile", "Account");
            }

            ViewBag.Addresses = addresses;
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder()
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "Cart is empty.";
                return RedirectToAction(nameof(Cart));
            }

            var orderRequest = new CreateOrderRequest
            {
                Items = cart.Select(c => new CreateOrderItemRequest
                {
                    ItemId = c.ItemId,
                    Quantity = c.Quantity
                }).ToList()
            };

            var result = await _apiClient.CreateOrderAsync(orderRequest);
            if (!result.Success || result.Data == null)
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to place order. Please review stock limits.";
                return RedirectToAction(nameof(Cart));
            }

            Response.Cookies.Delete(CartCookieName);
            TempData["SuccessMessage"] = "Your order was successfully placed! Thank you for shopping with us.";
            return RedirectToAction(nameof(History));
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("History", "Orders") });
            }

            var orders = await _apiClient.GetOrdersAsync();
            return View(orders.OrderByDescending(o => o.OrderedDate).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Track(int id)
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
            {
                return RedirectToAction("Login", "Account");
            }

            var order = await _apiClient.GetOrderByIdAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction(nameof(History));
            }

            return View(order);
        }
    }
}
