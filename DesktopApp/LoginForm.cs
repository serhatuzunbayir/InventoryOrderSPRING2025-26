using DesktopApp.Models;
using DesktopApp.Services;

namespace DesktopApp;

public partial class LoginForm : Form
{
    public ApiClient? ApiClient { get; private set; }
    public string StaffUsername { get; private set; } = string.Empty;

    public LoginForm()
    {
        // Login and registration actions.
        InitializeComponent();
        btnLogin.Click += async (_, _) => await HandleLoginAsync();
        btnRegister.Click += async (_, _) => await HandleRegisterAsync();
    }

    // Authenticate a staff user and pass the API client to the main form.
    private async Task HandleLoginAsync()
    {
        var baseUrl = txtBaseUrl.Text.Trim();
        if (!IsValidBaseUrl(baseUrl))
        {
            MessageBox.Show("Enter a valid API base URL (http/https).", "Invalid URL", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtLoginUser.Text) || string.IsNullOrWhiteSpace(txtLoginPass.Text))
        {
            MessageBox.Show("Username and password are required.", "Missing Fields", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var client = new ApiClient(baseUrl);
        var result = await client.LoginAsync(new LoginRequest
        {
            Username = txtLoginUser.Text.Trim(),
            Password = txtLoginPass.Text
        });

        if (!result.Success || result.Data == null)
        {
            MessageBox.Show(result.Error, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!string.Equals(result.Data.UserType, "Staff", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("Only staff accounts can access this app.", "Access Denied", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        client.SetToken(result.Data.Token);
        ApiClient = client;
        StaffUsername = txtLoginUser.Text.Trim();
        DialogResult = DialogResult.OK;
        Close();
    }

    // Register a new staff account for testing and keep the user on the login screen.
    private async Task HandleRegisterAsync()
    {
        var baseUrl = txtBaseUrl.Text.Trim();
        if (!IsValidBaseUrl(baseUrl))
        {
            MessageBox.Show("Enter a valid API base URL (http/https).", "Invalid URL", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtRegUser.Text) || string.IsNullOrWhiteSpace(txtRegPass.Text))
        {
            MessageBox.Show("Username and password are required.", "Missing Fields", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var client = new ApiClient(baseUrl);
        var result = await client.RegisterAsync(new RegisterRequest
        {
            Username = txtRegUser.Text.Trim(),
            Password = txtRegPass.Text,
            Email = txtRegEmail.Text.Trim(),
            PhoneNumber = txtRegPhone.Text.Trim(),
            FirstName = txtRegFirst.Text.Trim(),
            LastName = txtRegLast.Text.Trim(),
            UserType = "Staff"
        });

        if (!result.Success)
        {
            MessageBox.Show(result.Error, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        MessageBox.Show("Staff account created. You can now log in.", "Registration Complete",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Validate that the base URL is a valid HTTP(S) endpoint.
    private static bool IsValidBaseUrl(string baseUrl)
    {
        return Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri) &&
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
