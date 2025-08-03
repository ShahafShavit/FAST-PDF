using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

public partial class UpdateDialog : Form
{
    private readonly bool _forceUpdate;

    public UpdateDialog(bool forceUpdate = false)
    {
        InitializeComponent();
        _forceUpdate = forceUpdate;
    }

    private async void UpdateDialog_Load(object sender, EventArgs e)
    {
        await Updater.CheckForUpdates(progressBar1, labelStatus, downloadSizeLabel, speedLabel, timeRemainingLabel, _forceUpdate);

        if (!this.IsDisposed)
        {
            this.Close();
        }
    }

    public static class Updater
    {
        public static async Task CheckForUpdates(ProgressBar progressBar, Label statusLabel, Label sizeLabel, Label speedLabel, Label timeLabel, bool forceUpdate = false)
        {
            bool requireUpdate = false;

            if (forceUpdate)
            {
                requireUpdate = true;
                MessageBox.Show("Forcing update as requested. Installing the latest version from the server.");
            }
            else
            {
                try
                {
                    string currentVersionStr = AboutBox.AssemblyVersion;
                    string updateVersionUrl = "https://fast.shahafshavit.com/updates/version.txt";
                    string latestVersionStr;
                    using (var client = new WebClient())
                    {
                        latestVersionStr = await client.DownloadStringTaskAsync(updateVersionUrl);
                    }

                    var localVersion = new Version(currentVersionStr);
                    var remoteVersion = new Version(latestVersionStr.Trim());

                    if (remoteVersion > localVersion)
                    {
                        requireUpdate = true;
                        MessageBox.Show("Found a new update. Installing.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error checking for updates: {ex.Message}");
                    return; 
                }
            }

            if (requireUpdate)
            {
                try
                {
                    string zipUrl = "https://fast.shahafshavit.com/updates/FAST-PDF_latest.zip";
                    string tempPath = Path.Combine(Path.GetTempPath(), "FAST-PDF_Update");
                    string tempZipPath = Path.Combine(tempPath, "FAST-PDF_update.zip");
                    string appPath = AppDomain.CurrentDomain.BaseDirectory;
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

                    MessageBox.Show("Update downloaded. The application will now restart.");
                    RestartWithBatch();
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during the update process: {ex.Message}");
                }
            }
            else
            {
                UpdateStatus("FAST-PDF is up-to-date.", statusLabel);
                await Task.Delay(1500);
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

                    double bytesInMb = e.TotalBytesToReceive / 1024d / 1024d;
                    double downloadedMb = e.BytesReceived / 1024d / 1024d;
                    sizeLabel.Text = $"Size: {downloadedMb:F2} MB / {bytesInMb:F2} MB";

                    double speed = e.BytesReceived / 1024d / stopwatch.Elapsed.TotalSeconds;
                    string speedName = "KB/s";
                    if (speed > 1024d)
                    {
                        speed /= 1024d;
                        speedName = "MB/s";
                    }
                    speedLabel.Text = $"Speed: {speed:F2} {speedName}";

                    if (speed > 0)
                    {
                        double timeRemainingSeconds = (e.TotalBytesToReceive - e.BytesReceived) / (speed * 1024);
                        timeLabel.Text = $"Time Remaining: {timeRemainingSeconds:F0} seconds";
                    }
                };

                await webClient.DownloadFileTaskAsync(new Uri(url), destinationPath);
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
                writer.WriteLine("timeout /t 2 /nobreak > nul");
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