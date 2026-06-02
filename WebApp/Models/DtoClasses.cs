using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

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

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
    }

    public class RegisterResponse
    {
        public string Message { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }

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

    public class UpdateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class AddressDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AddressName { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
    }

    public class AddressRequest
    {
        public string AddressName { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderedDate { get; set; }
        public string Status { get; set; } = "Pending";
        public List<OrderItemDto> OrderItems { get; set; } = [];
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public ItemDto? Item { get; set; }
    }

    public class CreateOrderRequest
    {
        public List<CreateOrderItemRequest> Items { get; set; } = [];
    }

    public class CreateOrderItemRequest
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
    
    public class CartItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
