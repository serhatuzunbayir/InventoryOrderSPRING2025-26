using Backend.Data;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")!;
if (connectionString.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
{
    var pathPart = connectionString["Data Source=".Length..].Trim();
    if (!Path.IsPathRooted(pathPart))
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var resolvedPath = Path.Combine(baseDir, pathPart);
        
        var searchDir = baseDir;
        for (int i = 0; i < 5; i++)
        {
            var candidate = Path.Combine(searchDir, pathPart);
            if (File.Exists(candidate))
            {
                resolvedPath = candidate;
                break;
            }
            var parent = Directory.GetParent(searchDir);
            if (parent == null) break;
            searchDir = parent.FullName;
        }
        connectionString = $"Data Source={resolvedPath}";
    }
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// JWT authentication
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();
// Ignore circular references from EF Core navigation properties
builder.Services.AddControllers().AddJsonOptions(opts =>
    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReportsService>();

// Daily automatic database backup (every night at 00:00)
builder.Services.AddHostedService<BackupService>();

var app = builder.Build();

// Create the database automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
