using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;

public partial class UpdateDialog : Form
{
    private Stopwatch _stopwatch;
    public UpdateDialog()
    {
        InitializeComponent();
        _stopwatch = new Stopwatch();
    }

    private async void UpdateDialog_Load(object sender, EventArgs e)
    {
        // Check for updates when the dialog loads
        await Updater.CheckForUpdates(progressBar1, labelStatus, downloadSizeLabel, speedLabel, timeRemainingLabel);

        // Close the dialog after the update process completes or determines no updates are needed
        this.Close();
    }

    public static class Updater
    {
        public static async Task CheckForUpdates(ProgressBar progressBar, Label statusLabel, Label sizeLabel, Label speedLabel, Label timeLabel)
        {
            string currentVersion = AboutBox.AssemblyVersion;
            string updateVersionUrl = "https://fast.shahafshavit.com/updates/version.txt";
            string latestVersion = await new WebClient().DownloadStringTaskAsync(updateVersionUrl);

            if (latestVersion.Trim() != currentVersion)
            {
                string zipUrl = "https://fast.shahafshavit.com/updates/FAST-PDF_latest.zip";
                string tempPath = Path.Combine(Path.GetTempPath(), "FAST-PDF_Update");
                string tempZipPath = Path.Combine(tempPath, "FAST-PDF_update.zip");
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                MessageBox.Show(appPath);
                string updatePath = Path.Combine(tempPath, "extracted");

                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);

                UpdateStatus("Downloading update...", statusLabel);

                await DownloadFileWithProgressAsync(zipUrl, tempZipPath, progressBar, sizeLabel, speedLabel, timeLabel);

                UpdateStatus("Extracting update...", statusLabel);
                

                if (Directory.Exists(updatePath))
                    Directory.Delete(updatePath, true);

                System.IO.Compression.ZipFile.ExtractToDirectory(tempZipPath, updatePath, true);

                UpdateStatus("Preparing update...", statusLabel);
                CreateBatchFile(updatePath, appPath);

                MessageBox.Show("Update downloaded. Restarting...");
                RestartWithBatch();
                Environment.Exit(0);
            }
            else
            {
                UpdateStatus("FAST-PDF is up-to-date.", statusLabel);
            }
        }

        private static async Task DownloadFileWithProgressAsync(string url, string destinationPath, ProgressBar progressBar, Label sizeLabel, Label speedLabel, Label timeLabel)
        {
            using (var webClient = new WebClient())
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                webClient.DownloadProgressChanged += (sender, e) =>
                {
                    progressBar.Value = e.ProgressPercentage;

                    // Update size
                    double bytesInMb = e.TotalBytesToReceive / 1024d / 1024d;
                    double downloadedMb = e.BytesReceived / 1024d / 1024d;
                    sizeLabel.Text = $"Size: {downloadedMb:F2} MB / {bytesInMb:F2} MB";

                    // Update speed
                    double speed = e.BytesReceived / 1024d / stopwatch.Elapsed.TotalSeconds;
                    string speedName = "KB/s";
                    if (speed > 1024d)
                    {
                        speed /= 1024d;
                        speedName = "MB/s";
                    }
                    speedLabel.Text = $"Speed: {speed:F2} " + speedName;

                    // Update time remaining
                    double timeRemaining = (e.TotalBytesToReceive - e.BytesReceived) / speed;
                    timeLabel.Text = $"Time Remaining: {timeRemaining:F2} seconds";
                };

                await webClient.DownloadFileTaskAsync(url, destinationPath);
                stopwatch.Stop();
            }
        }

        private static void UpdateStatus(string message, Label statusLabel)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new Action(() => statusLabel.Text = message));
            }
            else
            {
                statusLabel.Text = message;
            }
        }

        private static void CreateBatchFile(string updatePath, string appPath)
        {
            string batchPath = Path.Combine(Path.GetTempPath(), "Update_FAST-PDF.bat");
            using (StreamWriter writer = new StreamWriter(batchPath))
            {
                writer.WriteLine("@echo off");
                writer.WriteLine("timeout /t 2 > nul");
                writer.WriteLine($"xcopy /e /y \"{updatePath}\\*\" \"{appPath}\"");
                writer.WriteLine($"start \"\" \"{Path.Combine(appPath, "FAST-PDF.exe")}\"");
                writer.WriteLine("del \"%~f0\" & exit");
            }
        }

        private static void RestartWithBatch()
        {
            string batchPath = Path.Combine(Path.GetTempPath(), "Update_FAST-PDF.bat");
            Process.Start(new ProcessStartInfo
            {
                FileName = batchPath,
                CreateNoWindow = true,
                UseShellExecute = false,
            });
        }
    }
}



