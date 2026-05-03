namespace DesktopApp.Models;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public int Id { get; set; }
    public string UserType { get; set; } = string.Empty;
}

public class RegisterResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
}

