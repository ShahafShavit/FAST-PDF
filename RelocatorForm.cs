using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public partial class RelocatorForm : Form
{
    public RelocatorForm(InputField inputField, UIConfig config)
    {
        InitializeComponent();
        int locationsCount = inputField.PDFSettings.Location.Count;
        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = locationsCount,
            RowCount = 1,
            RowStyles =
            {
                new RowStyle(SizeType.Percent, 100),
            },
            AutoSize = true,
        };
        int column = 0;
        for (int i = 0; i < locationsCount; i++)
        {
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / locationsCount));
            Relocator relocator = new Relocator(inputField.PDFSettings.Location[i]);
            relocator.Dock = DockStyle.Fill;
            relocator.AutoSize = true;
            relocator.Anchor = AnchorStyles.None;
            tableLayoutPanel.Controls.Add(relocator, column, 0);
            column++;
        }
        this.Controls.Add(tableLayoutPanel);
        this.AutoSize = true;
        this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.FormClosing += (s, e) =>
        {
            Config.Update(config);
        };
    }


}

