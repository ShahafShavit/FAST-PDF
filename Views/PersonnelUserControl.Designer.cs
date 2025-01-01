
    partial class PersonnelUserControl
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
        splitContainer1 = new SplitContainer();
        dataGridView1 = new DataGridView();
        tableLayoutPanel1 = new TableLayoutPanel();
        upRowBtn = new Button();
        downRowBtn = new Button();
        deleteRowBtn = new Button();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        tableLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 0);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(dataGridView1);
        splitContainer1.Panel1.RightToLeft = RightToLeft.Yes;
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(tableLayoutPanel1);
        splitContainer1.Panel2.RightToLeft = RightToLeft.Yes;
        splitContainer1.Size = new Size(816, 421);
        splitContainer1.SplitterDistance = 646;
        splitContainer1.TabIndex = 0;
        // 
        // dataGridView1
        // 
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.Location = new Point(0, 0);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.RowHeadersWidth = 51;
        dataGridView1.Size = new Size(646, 421);
        dataGridView1.TabIndex = 0;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 3;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
        tableLayoutPanel1.Controls.Add(upRowBtn, 1, 5);
        tableLayoutPanel1.Controls.Add(downRowBtn, 1, 6);
        tableLayoutPanel1.Controls.Add(deleteRowBtn, 1, 4);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 8;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
        tableLayoutPanel1.Size = new Size(166, 421);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // upRowBtn
        // 
        upRowBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        upRowBtn.Location = new Point(29, 263);
        upRowBtn.Name = "upRowBtn";
        upRowBtn.Size = new Size(110, 46);
        upRowBtn.TabIndex = 0;
        upRowBtn.Text = "העלה שורה";
        upRowBtn.UseVisualStyleBackColor = true;
        upRowBtn.Click += upRowBtn_Click;
        // 
        // downRowBtn
        // 
        downRowBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        downRowBtn.Location = new Point(29, 315);
        downRowBtn.Name = "downRowBtn";
        downRowBtn.Size = new Size(110, 46);
        downRowBtn.TabIndex = 1;
        downRowBtn.Text = "הורד שורה";
        downRowBtn.UseVisualStyleBackColor = true;
        downRowBtn.Click += downRowBtn_Click;
        // 
        // deleteRowBtn
        // 
        deleteRowBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        deleteRowBtn.Location = new Point(29, 211);
        deleteRowBtn.Name = "deleteRowBtn";
        deleteRowBtn.Size = new Size(110, 46);
        deleteRowBtn.TabIndex = 2;
        deleteRowBtn.Text = "מחק שורה";
        deleteRowBtn.UseVisualStyleBackColor = true;
        deleteRowBtn.Click += deleteRowBtn_Click;
        // 
        // PersonnelUserControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(splitContainer1);
        Name = "PersonnelUserControl";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(816, 421);
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        tableLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private SplitContainer splitContainer1;
    private DataGridView dataGridView1;
    private TableLayoutPanel tableLayoutPanel1;
    private Button upRowBtn;
    private Button downRowBtn;
    private Button deleteRowBtn;
}

