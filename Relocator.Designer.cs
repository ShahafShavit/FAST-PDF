
    partial class Relocator
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tableLayoutPanel1 = new TableLayoutPanel();
        up_btn = new Button();
        down_btn = new Button();
        left_btn = new Button();
        right_btn = new Button();
        tableLayoutPanel2 = new TableLayoutPanel();
        X_TB = new TextBox();
        label3 = new Label();
        Y_TB = new TextBox();
        label1 = new Label();
        label2 = new Label();
        PAGE_TB = new NumericUpDown();
        tableLayoutPanel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)PAGE_TB).BeginInit();
        SuspendLayout();
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 3;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.Controls.Add(up_btn, 1, 0);
        tableLayoutPanel1.Controls.Add(down_btn, 1, 2);
        tableLayoutPanel1.Controls.Add(left_btn, 0, 1);
        tableLayoutPanel1.Controls.Add(right_btn, 2, 1);
        tableLayoutPanel1.Location = new Point(0, 57);
        tableLayoutPanel1.Margin = new Padding(0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.Size = new Size(142, 142);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // up_btn
        // 
        up_btn.Dock = DockStyle.Fill;
        up_btn.Font = new Font("Segoe UI", 7F);
        up_btn.Location = new Point(50, 3);
        up_btn.Name = "up_btn";
        up_btn.Size = new Size(41, 41);
        up_btn.TabIndex = 0;
        up_btn.Text = "Up";
        up_btn.UseVisualStyleBackColor = true;
        // 
        // down_btn
        // 
        down_btn.Dock = DockStyle.Fill;
        down_btn.Font = new Font("Segoe UI", 7F);
        down_btn.Location = new Point(50, 97);
        down_btn.Name = "down_btn";
        down_btn.Size = new Size(41, 42);
        down_btn.TabIndex = 1;
        down_btn.Text = "Down";
        down_btn.UseVisualStyleBackColor = true;
        // 
        // left_btn
        // 
        left_btn.Dock = DockStyle.Fill;
        left_btn.Font = new Font("Segoe UI", 7F);
        left_btn.Location = new Point(3, 50);
        left_btn.Name = "left_btn";
        left_btn.Size = new Size(41, 41);
        left_btn.TabIndex = 2;
        left_btn.Text = "Left";
        left_btn.UseVisualStyleBackColor = true;
        // 
        // right_btn
        // 
        right_btn.Dock = DockStyle.Fill;
        right_btn.Font = new Font("Segoe UI", 7F);
        right_btn.Location = new Point(97, 50);
        right_btn.Name = "right_btn";
        right_btn.Size = new Size(42, 41);
        right_btn.TabIndex = 3;
        right_btn.Text = "Right";
        right_btn.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 3;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel2.Controls.Add(X_TB, 0, 1);
        tableLayoutPanel2.Controls.Add(label3, 2, 0);
        tableLayoutPanel2.Controls.Add(Y_TB, 0, 1);
        tableLayoutPanel2.Controls.Add(label1, 0, 0);
        tableLayoutPanel2.Controls.Add(label2, 1, 0);
        tableLayoutPanel2.Controls.Add(PAGE_TB, 2, 1);
        tableLayoutPanel2.Location = new Point(0, 0);
        tableLayoutPanel2.Margin = new Padding(0);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 2;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.Size = new Size(142, 56);
        tableLayoutPanel2.TabIndex = 1;
        // 
        // X_TB
        // 
        X_TB.Location = new Point(3, 31);
        X_TB.Name = "X_TB";
        X_TB.Size = new Size(41, 23);
        X_TB.TabIndex = 6;
        X_TB.TextAlign = HorizontalAlignment.Center;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Dock = DockStyle.Fill;
        label3.Location = new Point(97, 0);
        label3.Name = "label3";
        label3.Size = new Size(42, 28);
        label3.TabIndex = 4;
        label3.Text = "Page";
        label3.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // Y_TB
        // 
        Y_TB.Location = new Point(50, 31);
        Y_TB.Name = "Y_TB";
        Y_TB.Size = new Size(41, 23);
        Y_TB.TabIndex = 0;
        Y_TB.TextAlign = HorizontalAlignment.Center;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(3, 0);
        label1.Name = "label1";
        label1.Size = new Size(41, 28);
        label1.TabIndex = 2;
        label1.Text = "X";
        label1.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Dock = DockStyle.Fill;
        label2.Location = new Point(50, 0);
        label2.Name = "label2";
        label2.Size = new Size(41, 28);
        label2.TabIndex = 3;
        label2.Text = "Y";
        label2.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // PAGE_TB
        // 
        PAGE_TB.Location = new Point(97, 31);
        PAGE_TB.Name = "PAGE_TB";
        PAGE_TB.Size = new Size(42, 23);
        PAGE_TB.TabIndex = 7;
        // 
        // Relocator
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel2);
        Controls.Add(tableLayoutPanel1);
        Name = "Relocator";
        Size = new Size(142, 198);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)PAGE_TB).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private Button up_btn;
    private Button down_btn;
    private Button left_btn;
    private Button right_btn;
    private TableLayoutPanel tableLayoutPanel2;
    private TextBox Y_TB;
    private Label label1;
    private Label label2;
    private Label label3;
    private TextBox X_TB;
    private NumericUpDown PAGE_TB;
}

