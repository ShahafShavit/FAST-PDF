
internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        //Application.SetDefaultFont(new Font("Arial", 12));
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
#if DEBUG
        Application.Run(new Main());
#else
            using (var updateDialog = new UpdateDialog())
            {
                updateDialog.ShowDialog();
            }
            //}
            Application.Run(new Main());
#endif
    }
}
