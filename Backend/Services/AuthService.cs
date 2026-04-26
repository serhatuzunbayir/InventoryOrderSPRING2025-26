using Backend.Data;
using Backend.DTOs.Auth;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services;

public class AuthService(AppDbContext db, IConfiguration config)
{
    public async Task<ServiceResult<object>> RegisterAsync(RegisterRequest req)
    {
        bool exists = await db.Users.AnyAsync(u => u.Username == req.Username || u.Email == req.Email);
        if (exists)
            return ServiceResult<object>.Fail("Username or email already in use.");

        var user = new User
        {
            Username = req.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Email = req.Email,
            PhoneNumber = req.PhoneNumber,
            FirstName = req.FirstName,
            LastName = req.LastName,
            UserType = req.UserType
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return ServiceResult<object>.Ok(new { user.Id, user.Username, user.UserType });
    }

    public async Task<ServiceResult<object>> LoginAsync(LoginRequest req)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return ServiceResult<object>.Fail("Invalid username or password.");

        string token = GenerateJwt(user);
        return ServiceResult<object>.Ok(new { token, user.Id, user.UserType });
    }

    private string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.UserType)
        };

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddDays(7),
            claims: claims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
