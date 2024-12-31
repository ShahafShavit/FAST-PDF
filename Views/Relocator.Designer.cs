
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
        X_TB = new TextBox();
        Y_TB = new TextBox();
        PAGE_TB = new NumericUpDown();
        tableLayoutPanel2 = new TableLayoutPanel();
        label3 = new Label();
        label1 = new Label();
        label2 = new Label();
        RelocatorName = new Label();
        tableLayoutPanel3 = new TableLayoutPanel();
        tableLayoutPanel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)PAGE_TB).BeginInit();
        tableLayoutPanel2.SuspendLayout();
        tableLayoutPanel3.SuspendLayout();
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
        tableLayoutPanel1.Location = new Point(0, 102);
        tableLayoutPanel1.Margin = new Padding(0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
        tableLayoutPanel1.Size = new Size(162, 189);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // up_btn
        // 
        up_btn.Dock = DockStyle.Fill;
        up_btn.Font = new Font("Segoe UI", 7F);
        up_btn.Location = new Point(57, 4);
        up_btn.Margin = new Padding(3, 4, 3, 4);
        up_btn.Name = "up_btn";
        up_btn.Size = new Size(48, 55);
        up_btn.TabIndex = 3;
        up_btn.TabStop = false;
        up_btn.Text = "Up";
        up_btn.UseVisualStyleBackColor = true;
        // 
        // down_btn
        // 
        down_btn.Dock = DockStyle.Fill;
        down_btn.Font = new Font("Segoe UI", 7F);
        down_btn.Location = new Point(57, 130);
        down_btn.Margin = new Padding(3, 4, 3, 4);
        down_btn.Name = "down_btn";
        down_btn.Size = new Size(48, 55);
        down_btn.TabIndex = 4;
        down_btn.TabStop = false;
        down_btn.Text = "Down";
        down_btn.UseVisualStyleBackColor = true;
        // 
        // left_btn
        // 
        left_btn.Dock = DockStyle.Fill;
        left_btn.Font = new Font("Segoe UI", 7F);
        left_btn.Location = new Point(3, 67);
        left_btn.Margin = new Padding(3, 4, 3, 4);
        left_btn.Name = "left_btn";
        left_btn.Size = new Size(48, 55);
        left_btn.TabIndex = 5;
        left_btn.TabStop = false;
        left_btn.Text = "Left";
        left_btn.UseVisualStyleBackColor = true;
        // 
        // right_btn
        // 
        right_btn.Dock = DockStyle.Fill;
        right_btn.Font = new Font("Segoe UI", 7F);
        right_btn.Location = new Point(111, 67);
        right_btn.Margin = new Padding(3, 4, 3, 4);
        right_btn.Name = "right_btn";
        right_btn.Size = new Size(48, 55);
        right_btn.TabIndex = 6;
        right_btn.TabStop = false;
        right_btn.Text = "Right";
        right_btn.UseVisualStyleBackColor = true;
        // 
        // X_TB
        // 
        X_TB.Location = new Point(3, 41);
        X_TB.Margin = new Padding(3, 4, 3, 4);
        X_TB.Name = "X_TB";
        X_TB.Size = new Size(46, 27);
        X_TB.TabIndex = 0;
        X_TB.TextAlign = HorizontalAlignment.Center;
        // 
        // Y_TB
        // 
        Y_TB.Location = new Point(57, 41);
        Y_TB.Margin = new Padding(3, 4, 3, 4);
        Y_TB.Name = "Y_TB";
        Y_TB.Size = new Size(46, 27);
        Y_TB.TabIndex = 1;
        Y_TB.TextAlign = HorizontalAlignment.Center;
        // 
        // PAGE_TB
        // 
        PAGE_TB.Location = new Point(111, 41);
        PAGE_TB.Margin = new Padding(3, 4, 3, 4);
        PAGE_TB.Name = "PAGE_TB";
        PAGE_TB.Size = new Size(48, 27);
        PAGE_TB.TabIndex = 2;
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
        tableLayoutPanel2.Location = new Point(0, 27);
        tableLayoutPanel2.Margin = new Padding(0);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 2;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.Size = new Size(162, 75);
        tableLayoutPanel2.TabIndex = 7;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Dock = DockStyle.Fill;
        label3.Location = new Point(111, 0);
        label3.Name = "label3";
        label3.Size = new Size(48, 37);
        label3.TabIndex = 8;
        label3.Text = "Page";
        label3.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Dock = DockStyle.Fill;
        label1.Location = new Point(3, 0);
        label1.Name = "label1";
        label1.Size = new Size(48, 37);
        label1.TabIndex = 9;
        label1.Text = "X";
        label1.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Dock = DockStyle.Fill;
        label2.Location = new Point(57, 0);
        label2.Name = "label2";
        label2.Size = new Size(48, 37);
        label2.TabIndex = 10;
        label2.Text = "Y";
        label2.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // RelocatorName
        // 
        RelocatorName.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        RelocatorName.AutoSize = true;
        RelocatorName.Location = new Point(3, 0);
        RelocatorName.Name = "RelocatorName";
        RelocatorName.RightToLeft = RightToLeft.Yes;
        RelocatorName.Size = new Size(153, 22);
        RelocatorName.TabIndex = 8;
        RelocatorName.Text = "Filler";
        RelocatorName.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tableLayoutPanel3
        // 
        tableLayoutPanel3.ColumnCount = 1;
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.Controls.Add(RelocatorName, 0, 0);
        tableLayoutPanel3.Location = new Point(0, 2);
        tableLayoutPanel3.Name = "tableLayoutPanel3";
        tableLayoutPanel3.RowCount = 1;
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.Size = new Size(159, 22);
        tableLayoutPanel3.TabIndex = 8;
        // 
        // Relocator
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel3);
        Controls.Add(tableLayoutPanel2);
        Controls.Add(tableLayoutPanel1);
        Margin = new Padding(3, 4, 3, 4);
        Name = "Relocator";
        Size = new Size(162, 292);
        tableLayoutPanel1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)PAGE_TB).EndInit();
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        tableLayoutPanel3.ResumeLayout(false);
        tableLayoutPanel3.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel1;
    private TextBox X_TB;
    private TextBox Y_TB;
    private NumericUpDown PAGE_TB;
    private Button up_btn;
    private Button down_btn;
    private Button left_btn;
    private Button right_btn;
    private TableLayoutPanel tableLayoutPanel2;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label RelocatorName;
    private TableLayoutPanel tableLayoutPanel3;
}

