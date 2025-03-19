public partial class RelocatorForm : Form
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    public RelocatorForm(InputField inputField, Models models)
    {
        InitializeComponent();

        int locationsCount = inputField.Locations == null ? 0 : inputField.Locations.Count;

        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            RowStyles =
        {
            new RowStyle(SizeType.Percent, 90),
            new RowStyle(SizeType.Percent, 10),
        },
            AutoSize = true,
        };

        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 1,
            RowStyles =
        {
            new RowStyle(SizeType.Percent, 100),
        },
            AutoSize = true,
        };
        mainLayout.Controls.Add(tableLayoutPanel, 0, 0);
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
        if (inputField.Items != null)
        {
            foreach (var item in inputField.Items)
            {
                for (int i = 0; i < item.Locations.Count; i++)
                {
                    tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                    string relocatorLabel = item.Label ?? "";
                    Relocator relocator = new Relocator(item.Locations[i], relocatorLabel);
                    relocator.Dock = DockStyle.Fill;
                    relocator.AutoSize = true;
                    relocator.Anchor = AnchorStyles.None;
                    tableLayoutPanel.Controls.Add(relocator, column, 0);
                    column++;
                }
            }
        }

        // Create a panel for font settings.
        TableLayoutPanel fontController = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 2,
            ColumnStyles =
        {
            new ColumnStyle(SizeType.AutoSize),
            new ColumnStyle(SizeType.AutoSize),
        },
            AutoSize = true,
        };

        // Create and configure font size controls.
        Label fontSizeLabel = new Label { Text = "Font Size:" };
        NumericUpDown numericFontSize = new NumericUpDown { Minimum = 8, Maximum = 20, Value = 12, Dock = DockStyle.Fill };
        CheckBox useFontSizeCheckBox = new CheckBox { Text = "Use Font Size Function", Dock = DockStyle.Fill };

        numericFontSize.DataBindings.Add("Value", inputField, "Size", true, DataSourceUpdateMode.OnPropertyChanged);
        useFontSizeCheckBox.DataBindings.Add("Checked", inputField, "ResizeFunctionUse", true, DataSourceUpdateMode.OnPropertyChanged);
        // Add controls to the font controller panel.
        fontController.Controls.Add(fontSizeLabel, 0, 0);
        fontController.Controls.Add(numericFontSize, 1, 0);
        fontController.Controls.Add(useFontSizeCheckBox, 0, 1);
        fontController.SetColumnSpan(useFontSizeCheckBox, 2);

        mainLayout.Controls.Add(fontController, 0, 1);

        this.Controls.Add(mainLayout);
        this.AutoSize = true;
        this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        this.FormClosing += (s, e) =>
        {
            Config.UpdateModels(models);
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

