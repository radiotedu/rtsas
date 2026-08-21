using IcecastStreamMonitor.Forms;

namespace IcecastStreamMonitor;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            if (args.ExceptionObject is Exception ex)
            {
                MessageBox.Show(
                    $"An unexpected fatal error occurred:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        };

        Application.ThreadException += (sender, args) =>
        {
            MessageBox.Show(
                $"An error occurred:\n\n{args.Exception.Message}",
                "Stream Monitor Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        };

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }    
}