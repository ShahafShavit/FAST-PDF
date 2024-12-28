public partial class RelocatorForm : Form
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    public RelocatorForm(InputField inputField, Models models)
    {
        InitializeComponent();

        int locationsCount = inputField.Locations.Count;
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
            Relocator relocator = new Relocator(inputField.Locations[i]);
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
            Config.UpdateModels(models);
            //foreach (Control item in this.Controls)
            //{
            //    if (item is TableLayoutPanel tableLayoutPanel)
            //        foreach (Control control in tableLayoutPanel.Controls)
            //        {
            //            foreach (Control tbcontrol in control.Controls)
            //            {
            //                if (tbcontrol is TableLayoutPanel tlp)
            //                    foreach (Control item1 in tlp.Controls)
            //                    {
            //                        if (item1 is TextBox tb)
            //                        {

            //                        }
            //                    }

            //            }
            //        }
            //}
        };
    }


}

