using DesktopApp.Services;

namespace DesktopApp;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var login = new LoginForm();
        if (login.ShowDialog() == DialogResult.OK && login.ApiClient != null)
        {
            // Create notification service singleton.
            var notificationService = new NotificationService();
            var notificationCoordinator = new NotificationCoordinator(login.ApiClient, notificationService);
            Application.Run(new MainForm(login.ApiClient, login.StaffUsername, notificationCoordinator));
        }
    }
}
