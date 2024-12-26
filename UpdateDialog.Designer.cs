partial class UpdateDialog
{
    private System.ComponentModel.IContainer components = null;
    private ProgressBar progressBar1;
    private Label labelStatus;
    private Label downloadSizeLabel;
    private Label speedLabel;
    private Label timeRemainingLabel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.labelStatus = new System.Windows.Forms.Label();
        this.downloadSizeLabel = new System.Windows.Forms.Label();
        this.speedLabel = new System.Windows.Forms.Label();
        this.timeRemainingLabel = new System.Windows.Forms.Label();
        this.SuspendLayout();

        // ProgressBar
        this.progressBar1.Location = new System.Drawing.Point(12, 12);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(360, 23);
        this.progressBar1.TabIndex = 0;

        // Status Label
        this.labelStatus.AutoSize = true;
        this.labelStatus.Location = new System.Drawing.Point(12, 38);
        this.labelStatus.Name = "labelStatus";
        this.labelStatus.Size = new System.Drawing.Size(92, 15);
        this.labelStatus.TabIndex = 1;
        this.labelStatus.Text = "Checking updates...";

        // Download Size Label
        this.downloadSizeLabel.AutoSize = true;
        this.downloadSizeLabel.Location = new System.Drawing.Point(12, 58);
        this.downloadSizeLabel.Name = "downloadSizeLabel";
        this.downloadSizeLabel.Size = new System.Drawing.Size(82, 15);
        this.downloadSizeLabel.TabIndex = 2;
        this.downloadSizeLabel.Text = "Download Size:";

        // Speed Label
        this.speedLabel.AutoSize = true;
        this.speedLabel.Location = new System.Drawing.Point(12, 78);
        this.speedLabel.Name = "speedLabel";
        this.speedLabel.Size = new System.Drawing.Size(42, 15);
        this.speedLabel.TabIndex = 3;
        this.speedLabel.Text = "Speed:";

        // Time Remaining Label
        this.timeRemainingLabel.AutoSize = true;
        this.timeRemainingLabel.Location = new System.Drawing.Point(12, 98);
        this.timeRemainingLabel.Name = "timeRemainingLabel";
        this.timeRemainingLabel.Size = new System.Drawing.Size(99, 15);
        this.timeRemainingLabel.TabIndex = 4;
        this.timeRemainingLabel.Text = "Time Remaining:";

        // UpdateDialog
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(384, 130);
        //this.Controls.Add(this.timeRemainingLabel);
        this.Controls.Add(this.speedLabel);
        this.Controls.Add(this.downloadSizeLabel);
        this.Controls.Add(this.labelStatus);
        this.Controls.Add(this.progressBar1);
        this.Name = "UpdateDialog";
        this.Text = "FAST PDF Update";
        this.Load += new System.EventHandler(this.UpdateDialog_Load);
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}