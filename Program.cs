using System;
using System.Linq;
using System.Windows.Forms;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();

        // Check if the "--force-update" argument was passed
        bool forceUpdate = args.Contains("--force-update");

#if DEBUG
        Application.Run(new Main());
#else
        // Pass the flag to your update dialog
        using (var updateDialog = new UpdateDialog(forceUpdate))
        {
            updateDialog.ShowDialog();
        }
        Application.Run(new Main());
#endif
    }
}