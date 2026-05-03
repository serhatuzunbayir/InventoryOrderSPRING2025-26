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
            Application.Run(new MainForm(login.ApiClient, login.StaffUsername));
        }
    }
}