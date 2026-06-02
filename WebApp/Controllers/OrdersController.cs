using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    // OrdersController handles shopping cart updates, checkout processing, order placement, and tracking.
    public class OrdersController : Controller
    {
        private readonly BackendApiClient _apiClient;
        private readonly ILogger<OrdersController> _logger;
        private const string CartCookieName = "shopping_cart";

        // Constructor to inject API client and logging services.
        public OrdersController(BackendApiClient apiClient, ILogger<OrdersController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        // Helper method: Retrieves the shopping cart from cookies and deserializes it.
        private List<CartItem> GetCart()
        {
            if (Request.Cookies.TryGetValue(CartCookieName, out var json) && !string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    // Deserialize the cart items list from JSON
                    return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
                }
                catch
                {
                    // Return empty list on deserialization failure
                    return new List<CartItem>();
                }
            }
            return new List<CartItem>();
        }

        // Helper method: Serializes the cart items list and saves it back into cookies.
        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            Response.Cookies.Append(CartCookieName, json, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(14)
            });
        }

        // GET: Displays the current shopping cart view.
        [HttpGet]
        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        // POST: Adds a selected quantity of a product to the shopping cart.
        [HttpPost]
        public async Task<IActionResult> AddToCart(int itemId, int quantity = 1)
        {
            // Verify that the item exists on the backend API
            var item = await _apiClient.GetItemByIdAsync(itemId);
            if (item == null)
            {
                TempData["ErrorMessage"] = "Item not found.";
                return RedirectToAction("Index", "Home");
            }

            // Verify that backend stock level supports the requested quantity
            if (item.Quantity < quantity)
            {
                TempData["ErrorMessage"] = $"Cannot add {quantity} of '{item.Name}'. Only {item.Quantity} in stock.";
                return RedirectToAction("Index", "Home");
            }

            var cart = GetCart();
            var existing = cart.FirstOrDefault(i => i.ItemId == itemId);
            if (existing != null)
            {
                // Check if combined cart quantity exceeds available stock limit
                if (item.Quantity < existing.Quantity + quantity)
                {
                    TempData["ErrorMessage"] = $"Cannot add more. Total cart quantity would exceed stock limit of {item.Quantity}.";
                    return RedirectToAction("Index", "Home");
                }
                existing.Quantity += quantity;
            }
            else
            {
                // Add a new product entry to the cart list
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

        // POST: Drops a specific quantity of an item from the cart.
        [HttpPost]
        public IActionResult RemoveFromCart(int itemId, int quantity = 1)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ItemId == itemId);
            if (item != null)
            {
                // Fully remove the item if the dropped quantity matches or exceeds what's in cart
                if (item.Quantity <= quantity)
                {
                    cart.Remove(item);
                    TempData["SuccessMessage"] = $"Removed '{item.Name}' from cart.";
                }
                else
                {
                    // Otherwise decrement the quantity
                    item.Quantity -= quantity;
                    TempData["SuccessMessage"] = $"Reduced '{item.Name}' quantity by {quantity}.";
                }
                SaveCart(cart);
            }
            return RedirectToAction(nameof(Cart));
        }

        // POST: Clears all items in the shopping cart.
        [HttpPost]
        public IActionResult ClearCart()
        {
            Response.Cookies.Delete(CartCookieName);
            TempData["SuccessMessage"] = "Cart cleared.";
            return RedirectToAction(nameof(Cart));
        }

        // GET: Displays the checkout screen showing cart items and shipping addresses.
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            // Verify active login token
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

            // Retrieve registered shipping addresses of the customer
            var addresses = await _apiClient.GetAddressesAsync();
            if (addresses.Count == 0)
            {
                TempData["ErrorMessage"] = "Please add at least one shipping address in your profile before checking out.";
                return RedirectToAction("Profile", "Account");
            }

            ViewBag.Addresses = addresses;
            return View(cart);
        }

        // POST: Places the order on the backend API and clears the local cookie cart.
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

            // Map cart items to DTO requests
            var orderRequest = new CreateOrderRequest
            {
                Items = cart.Select(c => new CreateOrderItemRequest
                {
                    ItemId = c.ItemId,
                    Quantity = c.Quantity
                }).ToList()
            };

            // Call backend API to place the order
            var result = await _apiClient.CreateOrderAsync(orderRequest);
            if (!result.Success || result.Data == null)
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to place order. Please review stock limits.";
                return RedirectToAction(nameof(Cart));
            }

            // Success: clear cart and redirect to order history
            Response.Cookies.Delete(CartCookieName);
            TempData["SuccessMessage"] = "Your order was successfully placed! Thank you for shopping with us.";
            return RedirectToAction(nameof(History));
        }

        // GET: Displays customer order history.
        [HttpGet]
        public async Task<IActionResult> History()
        {
            if (!Request.Cookies.ContainsKey("jwt_token"))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("History", "Orders") });
            }

            // Retrieve and display orders sorted chronologically in reverse order
            var orders = await _apiClient.GetOrdersAsync();
            return View(orders.OrderByDescending(o => o.OrderedDate).ToList());
        }

        // GET: Displays shipping tracker status details for a specific order.
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
