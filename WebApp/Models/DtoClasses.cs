using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    // DtoClasses contains standard data transfer objects used to exchange information with the Backend API.

    // Request payload containing credentials for logging in.
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Request payload containing fields for registering a new user.
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserType { get; set; } = "Customer";
    }

    // Response payload returned upon successful login, containing the auth token and username.
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
    }

    // Response payload containing registration results.
    public class RegisterResponse
    {
        public string Message { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    // Represents an inventory product item.
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    // Represents user contact and profile details.
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
    }

    // Request payload containing updated user contact info fields.
    public class UpdateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    // Represents a registered customer shipping address.
    public class AddressDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AddressName { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
    }

    // Request payload containing fields to add or modify a shipping address.
    public class AddressRequest
    {
        public string AddressName { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
    }

    // Represents an order header details.
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderedDate { get; set; }
        public string Status { get; set; } = "Pending";
        public List<OrderItemDto> OrderItems { get; set; } = [];
    }

    // Represents a product item line inside an order.
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public ItemDto? Item { get; set; }
    }

    // Request payload used to create a new order.
    public class CreateOrderRequest
    {
        public List<CreateOrderItemRequest> Items { get; set; } = [];
    }

    // Represents a single item request inside a new order request.
    public class CreateOrderItemRequest
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
    
    // Represents a product item stored in the shopping cart cookie list.
    public class CartItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
