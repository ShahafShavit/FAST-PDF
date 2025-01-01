
partial class PersonnelForm
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tabPage3 = new TabPage();
        splitContainer2 = new SplitContainer();
        clientsGridView = new DataGridView();
        tabPage1 = new TabPage();
        splitContainer1 = new SplitContainer();
        personnelGridView = new DataGridView();
        tabControl1 = new TabControl();
        tabPage3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
        splitContainer2.Panel2.SuspendLayout();
        splitContainer2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)clientsGridView).BeginInit();
        tabPage1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)personnelGridView).BeginInit();
        tabControl1.SuspendLayout();
        SuspendLayout();
        // 
        // tabPage3
        // 
        tabPage3.Controls.Add(splitContainer2);
        tabPage3.Location = new Point(4, 29);
        tabPage3.Name = "tabPage3";
        tabPage3.Padding = new Padding(3);
        tabPage3.Size = new Size(995, 318);
        tabPage3.TabIndex = 2;
        tabPage3.Text = "tabPage3";
        tabPage3.UseVisualStyleBackColor = true;
        // 
        // splitContainer2
        // 
        splitContainer2.Dock = DockStyle.Fill;
        splitContainer2.Location = new Point(3, 3);
        splitContainer2.Name = "splitContainer2";
        // 
        // splitContainer2.Panel1
        // 
        splitContainer2.Panel1.RightToLeft = RightToLeft.Yes;
        // 
        // splitContainer2.Panel2
        // 
        splitContainer2.Panel2.Controls.Add(clientsGridView);
        splitContainer2.Panel2.RightToLeft = RightToLeft.Yes;
        splitContainer2.Size = new Size(989, 312);
        splitContainer2.SplitterDistance = 178;
        splitContainer2.TabIndex = 0;
        // 
        // clientsGridView
        // 
        clientsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        clientsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        clientsGridView.Dock = DockStyle.Fill;
        clientsGridView.Location = new Point(0, 0);
        clientsGridView.Name = "clientsGridView";
        clientsGridView.RowHeadersWidth = 51;
        clientsGridView.Size = new Size(807, 312);
        clientsGridView.TabIndex = 0;
        // 
        // tabPage1
        // 
        tabPage1.Controls.Add(splitContainer1);
        tabPage1.Location = new Point(4, 29);
        tabPage1.Name = "tabPage1";
        tabPage1.Padding = new Padding(3);
        tabPage1.Size = new Size(995, 318);
        tabPage1.TabIndex = 0;
        tabPage1.Text = "tabPage1";
        tabPage1.UseVisualStyleBackColor = true;
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(3, 3);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.RightToLeft = RightToLeft.Yes;
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(personnelGridView);
        splitContainer1.Panel2.RightToLeft = RightToLeft.Yes;
        splitContainer1.Size = new Size(989, 312);
        splitContainer1.SplitterDistance = 178;
        splitContainer1.TabIndex = 0;
        // 
        // personnelGridView
        // 
        personnelGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        personnelGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        personnelGridView.Dock = DockStyle.Fill;
        personnelGridView.Location = new Point(0, 0);
        personnelGridView.Name = "personnelGridView";
        personnelGridView.RowHeadersWidth = 51;
        personnelGridView.Size = new Size(807, 312);
        personnelGridView.TabIndex = 0;
        // 
        // tabControl1
        // 
        tabControl1.Controls.Add(tabPage1);
        tabControl1.Controls.Add(tabPage3);
        tabControl1.Dock = DockStyle.Fill;
        tabControl1.Location = new Point(0, 0);
        tabControl1.Name = "tabControl1";
        tabControl1.RightToLeft = RightToLeft.Yes;
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new Size(1003, 351);
        tabControl1.TabIndex = 0;
        // 
        // PersonnelForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1003, 351);
        Controls.Add(tabControl1);
        Name = "PersonnelForm";
        Text = "PersonnelForm";
        tabPage3.ResumeLayout(false);
        splitContainer2.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
        splitContainer2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)clientsGridView).EndInit();
        tabPage1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)personnelGridView).EndInit();
        tabControl1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TabPage tabPage3;
    private SplitContainer splitContainer2;
    private DataGridView clientsGridView;
    private TabPage tabPage1;
    private SplitContainer splitContainer1;
    private DataGridView personnelGridView;
    private TabControl tabControl1;
}
