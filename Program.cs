//using squ

using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Auto_UI_Test
{

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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            /*
            using (var updateDialog = new UpdateDialog())
            {
                updateDialog.ShowDialog();
            }
            */
            Application.Run(new Main());
        }
    }
}