using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DesktopApp.Models;

namespace DesktopApp.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public string BaseUrl { get; }

    public ApiClient(string baseUrl)
    {
        // Configure the base URL and default headers for JSON API calls.
        BaseUrl = baseUrl.TrimEnd('/');
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
    }

    // Attach the JWT token to subsequent requests.
    public void SetToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // Submit staff credentials and return a JWT token response.
    public async Task<ApiResult<AuthResponse>> LoginAsync(LoginRequest request)
    {
        return await PostAsync<LoginRequest, AuthResponse>("/api/auth/login", request);
    }

    // Register a staff user in the backend.
    public async Task<ApiResult<RegisterResponse>> RegisterAsync(RegisterRequest request)
    {
        return await PostAsync<RegisterRequest, RegisterResponse>("/api/auth/register", request);
    }

    // Fetch the full inventory list.
    public async Task<ApiResult<List<ItemDto>>> GetItemsAsync()
    {
        return await GetAsync<List<ItemDto>>("/api/items");
    }

    // Create a new inventory item.
    public async Task<ApiResult<ItemDto>> CreateItemAsync(ItemRequest request)
    {
        return await PostAsync<ItemRequest, ItemDto>("/api/items", request);
    }

    // Update an existing inventory item.
    public async Task<ApiResult<ItemDto>> UpdateItemAsync(int id, ItemRequest request)
    {
        return await PutAsync<ItemRequest, ItemDto>($"/api/items/{id}", request);
    }

    // Delete an inventory item by id.
    public async Task<ApiResult<bool>> DeleteItemAsync(int id)
    {
        return await DeleteAsync($"/api/items/{id}");
    }

    // Fetch all orders for staff viewing.
    public async Task<ApiResult<List<OrderDto>>> GetOrdersAsync()
    {
        return await GetAsync<List<OrderDto>>("/api/orders");
    }

    // Update an order status for shipping management.
    public async Task<ApiResult<OrderDto>> UpdateOrderStatusAsync(int id, UpdateOrderStatusRequest request)
    {
        return await PutAsync<UpdateOrderStatusRequest, OrderDto>($"/api/orders/{id}/status", request);
    }

    // Fetch the sales report.
    public async Task<ApiResult<SalesReportDto>> GetSalesReportAsync()
    {
        return await GetAsync<SalesReportDto>("/api/reports/sales");
    }

    // Build a GET request and send it through the shared pipeline.
    private async Task<ApiResult<TResponse>> GetAsync<TResponse>(string path)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        return await SendAsync<TResponse>(request);
    }

    // Build a POST request with JSON payload and send it.
    private async Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string path, TRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json");
        using var message = new HttpRequestMessage(HttpMethod.Post, path) { Content = content };
        return await SendAsync<TResponse>(message);
    }

    // Build a PUT request with JSON payload and send it.
    private async Task<ApiResult<TResponse>> PutAsync<TRequest, TResponse>(string path, TRequest request)
    {
        var content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json");
        using var message = new HttpRequestMessage(HttpMethod.Put, path) { Content = content };
        return await SendAsync<TResponse>(message);
    }

    // Send a DELETE request and return success status.
    private async Task<ApiResult<bool>> DeleteAsync(string path)
    {
        using var message = new HttpRequestMessage(HttpMethod.Delete, path);
        var response = await _httpClient.SendAsync(message);
        if (response.IsSuccessStatusCode)
        {
            return ApiResult<bool>.Ok(true, response.StatusCode);
        }

        var errorText = await response.Content.ReadAsStringAsync();
        return ApiResult<bool>.Fail(errorText, response.StatusCode);
    }

    // Send an HTTP request and map the response to ApiResult.
    private async Task<ApiResult<TResponse>> SendAsync<TResponse>(HttpRequestMessage request)
    {
        var response = await _httpClient.SendAsync(request);
        var payload = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                return ApiResult<TResponse>.Ok(default, response.StatusCode);
            }

            var data = JsonSerializer.Deserialize<TResponse>(payload, _jsonOptions);
            return ApiResult<TResponse>.Ok(data, response.StatusCode);
        }

        var error = string.IsNullOrWhiteSpace(payload) ? response.ReasonPhrase ?? "Request failed." : payload;
        return ApiResult<TResponse>.Fail(error, response.StatusCode);
    }
}
