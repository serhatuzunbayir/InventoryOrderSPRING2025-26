using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Services
{
    public class BackendApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public BackendApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.BaseAddress = new Uri("http://localhost:5000");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void AddAuthHeader()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request.Cookies.TryGetValue("jwt_token", out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<ServiceResult<AuthResponse>> LoginAsync(LoginRequest req)
        {
            return await PostAsync<LoginRequest, AuthResponse>("/api/auth/login", req);
        }

        public async Task<ServiceResult<RegisterResponse>> RegisterAsync(RegisterRequest req)
        {
            return await PostAsync<RegisterRequest, RegisterResponse>("/api/auth/register", req);
        }

        public async Task<List<ItemDto>> GetItemsAsync(string? name = null, string? category = null, string? sortByPrice = null)
        {
            var url = $"/api/items?name={Uri.EscapeDataString(name ?? "")}&category={Uri.EscapeDataString(category ?? "")}&sortByPrice={Uri.EscapeDataString(sortByPrice ?? "")}";
            var result = await GetAsync<List<ItemDto>>(url);
            return result.Data ?? new List<ItemDto>();
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var result = await GetAsync<ItemDto>($"/api/items/{id}");
            return result.Data;
        }

        public async Task<UserProfileDto?> GetProfileAsync()
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return null;

            AddAuthHeader();
            var result = await GetAsync<UserProfileDto>($"/api/users/{userId}");
            return result.Data;
        }

        public async Task<bool> UpdateProfileAsync(UpdateUserRequest req)
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return false;

            AddAuthHeader();
            var response = await _httpClient.PutAsJsonAsync($"/api/users/{userId}", req);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<AddressDto>> GetAddressesAsync()
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return new List<AddressDto>();

            AddAuthHeader();
            var result = await GetAsync<List<AddressDto>>($"/api/users/{userId}/addresses");
            return result.Data ?? new List<AddressDto>();
        }

        public async Task<AddressDto?> AddAddressAsync(AddressRequest req)
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return null;

            AddAuthHeader();
            var result = await PostAsync<AddressRequest, AddressDto>($"/api/users/{userId}/addresses", req);
            return result.Data;
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return false;

            AddAuthHeader();
            var response = await _httpClient.DeleteAsync($"/api/users/{userId}/addresses/{addressId}");
            return response.IsSuccessStatusCode;
        }

        private int? GetUserIdFromToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Request.Cookies.TryGetValue("jwt_token", out var token) && !string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    var parts = token.Split('.');
                    if (parts.Length > 1)
                    {
                        var payload = parts[1];
                        payload = payload.Replace('-', '+').Replace('_', '/');
                        switch (payload.Length % 4)
                        {
                            case 2: payload += "=="; break;
                            case 3: payload += "="; break;
                        }
                        var bytes = Convert.FromBase64String(payload);
                        var json = Encoding.UTF8.GetString(bytes);
                        using var doc = JsonDocument.Parse(json);
                        
                        if (doc.RootElement.TryGetProperty("nameid", out var nameidProp))
                        {
                            if (int.TryParse(nameidProp.GetString(), out var id))
                                return id;
                        }
                        
                        if (doc.RootElement.TryGetProperty("sub", out var subProp))
                        {
                            if (int.TryParse(subProp.GetString(), out var id))
                                return id;
                        }
                        
                        foreach (var prop in doc.RootElement.EnumerateObject())
                        {
                            if (prop.Name.EndsWith("nameidentifier", StringComparison.OrdinalIgnoreCase) || prop.Name.Equals("nameid", StringComparison.OrdinalIgnoreCase))
                            {
                                if (int.TryParse(prop.Value.GetString(), out var id))
                                    return id;
                            }
                        }
                    }
                }
                catch {}
            }
            return null;
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            AddAuthHeader();
            var result = await GetAsync<List<OrderDto>>("/api/orders");
            return result.Data ?? new List<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            AddAuthHeader();
            var result = await GetAsync<OrderDto>($"/api/orders/{id}");
            return result.Data;
        }

        public async Task<ServiceResult<OrderDto>> CreateOrderAsync(CreateOrderRequest req)
        {
            AddAuthHeader();
            return await PostAsync<CreateOrderRequest, OrderDto>("/api/orders", req);
        }

        private async Task<ServiceResult<T>> GetAsync<T>(string url)
        {
            try
            {
                AddAuthHeader();
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonSerializer.Deserialize<T>(content, _jsonOptions);
                    return ServiceResult<T>.SuccessResult(data);
                }
                return ServiceResult<T>.FailureResult(content);
            }
            catch (Exception ex)
            {
                return ServiceResult<T>.FailureResult($"Connection error: {ex.Message}");
            }
        }

        private async Task<ServiceResult<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request)
        {
            try
            {
                AddAuthHeader();
                var json = JsonSerializer.Serialize(request, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
                    return ServiceResult<TResponse>.SuccessResult(data);
                }
                return ServiceResult<TResponse>.FailureResult(responseContent);
            }
            catch (Exception ex)
            {
                return ServiceResult<TResponse>.FailureResult($"Connection error: {ex.Message}");
            }
        }
    }

    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public static ServiceResult<T> SuccessResult(T? data) => new() { Success = true, Data = data };
        public static ServiceResult<T> FailureResult(string? error) => new() { Success = false, ErrorMessage = error };
    }
}
