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

        while (true)
        {
            using var login = new LoginForm();
            if (login.ShowDialog() != DialogResult.OK || login.ApiClient == null)
            {
                break;
            }

            var optionsService = new AppOptionsService();
            var options = optionsService.Load();

            // Create notification service singleton.
            var notificationService = new NotificationService(options.LowStockThreshold);
            var notificationCoordinator = new NotificationCoordinator(login.ApiClient, notificationService, options);
            using var mainForm = new MainForm(
                login.ApiClient,
                login.StaffUsername,
                notificationCoordinator,
                optionsService,
                options);
            Application.Run(mainForm);

            if (!mainForm.LogoutRequested)
            {
                break;
            }
        }
    }
}
