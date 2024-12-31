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
            //ColumnCount = locationsCount,
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
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            Relocator relocator = new Relocator(inputField.Locations[i], i.ToString());
            relocator.Dock = DockStyle.Fill;
            relocator.AutoSize = true;
            relocator.Anchor = AnchorStyles.None;
            tableLayoutPanel.Controls.Add(relocator, column, 0);
            column++;
        }
        if (inputField.Items != null) // Case combobox selector
        {
            foreach (var item in inputField.Items)
            {
                for (int i = 0; i < item.Locations.Count; i++)
                {
                    tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                    string relocatorLabel = "";
                    if (item.Label != null) { relocatorLabel = item.Label; }
                    Relocator relocator = new Relocator(item.Locations[i], relocatorLabel);
                    relocator.Dock = DockStyle.Fill;
                    relocator.AutoSize = true;
                    relocator.Anchor = AnchorStyles.None;
                    tableLayoutPanel.Controls.Add(relocator, column, 0 );
                    column++;
                }
            }
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

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // Check if the pressed key is the Escape key
        if (keyData == Keys.Escape)
        {
            this.Close(); // Close the form, simulating the X button
            return true;  // Indicate the key press has been handled
        }

        return base.ProcessCmdKey(ref msg, keyData); // Call the base method for other keys
    }

}

