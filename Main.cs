//using FAST;
using iText.Kernel.Colors;
using iText.Kernel.Utils.Objectpathitems;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
/*
#pragma warning disable CS8602
#pragma warning disable CS8618
#pragma warning disable CS8604
#pragma warning disable CS8622
#pragma warning disable CS8600*/
public partial class Main : System.Windows.Forms.Form
{
    public System.Drawing.Color mainColor;
    private ColorDialog mainColorDialog = new ColorDialog();
    private TextBox console;
    private ClientsList clients;
    private Personnel personnel;
    private GlobalSettings globalSettings;
    private Models models;
    private bool debug;
    private const int MAX_FORMS_PER_PAGE = 5;
    private const int MIN_FORMS_PER_PAGE = 3;
    private const int MIN_FORM_WIDTH = 400;
    private const int SPACE_PER_INPUT = 65;
    private const int ELEMENT_PADDING = 3;
    public Main()
    {
        InitializeComponent();
        try
        {
            this.globalSettings = Config.PullSettings();
            this.models = Config.PullModels();
            this.personnel = Config.PullPersonnel();
            this.clients = Config.PullClients();
        }
        catch (Exception e)
        { MessageBox.Show("Error: " + e.Message); Environment.Exit(1); }
        this.debug = this.globalSettings.Debug;
        this.Text = "מערכת מילוי טפסים- להט הנדסת חשמל";

        this.RightToLeft = RightToLeft.Yes;
        this.AutoScaleMode = AutoScaleMode.Dpi;
        this.AutoSize = true;
        this.AutoSizeMode = AutoSizeMode.GrowOnly;



        GenerateUI();

        Console.WriteLine("Initialization of components has been completed.");
        this.FormClosing += (o, e) =>
        {
            Config.UpdateModels(models);
            if (debug)
            {
                Config.DeveloperSwap();
            }
        };
        this.ToggleHighContrast(false);
    }
    private void GenerateUI()
    {
        // Create the main layout and attach it to the form
        TableLayoutPanel mainLayout = CreateMainLayout();
        MenuStrip mainMenuStrip = GenerateMenuStrip();
        TabControl tabControl = CreateTabControl();
        this.console = CreateConsoleTextBox();

        // Build tab pages
        PopulateTabControl(tabControl);

        // Arrange the controls within the main layout
        mainLayout.Controls.Add(mainMenuStrip, 0, 0);
        mainLayout.Controls.Add(tabControl, 0, 1);
        mainLayout.Controls.Add(this.console, 0, 2);

        this.Controls.Add(mainLayout);

        // Redirect console output to the TextBox
        Console.SetOut(new TextBoxWriter(console));

        // Calculate initial window size
        CalculateWindowSize();
    }

    // ------------------------ Sub-Methods ------------------------ //

    private TableLayoutPanel CreateMainLayout()
    {
        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));     // For MenuStrip
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // For TabControl
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));     // For Console
        return mainLayout;
    }

    private TabControl CreateTabControl()
    {
        var tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            RightToLeftLayout = true
        };
        return tabControl;
    }

    private TextBox CreateConsoleTextBox()
    {
        return new TextBox
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            ReadOnly = true,
            Multiline = true,
            Height = 75,
            ScrollBars = ScrollBars.Vertical,
            RightToLeft = RightToLeft.No
        };
    }

    private void PopulateTabControl(TabControl tabControl)
    {
        foreach (var tab in this.models.Tabs)
        {
            var tabPage = CreateTabPage(tab);
            tabControl.TabPages.Add(tabPage);
        }
    }

    private TabPage CreateTabPage(TabObject tab)
    {
        var tabPage = new TabPage
        {
            Margin = new Padding(25),
            Text = tab.TabName,
        };

        // Compute row/column counts, prepare layout
        var tabLayoutPanel = CreateTabLayoutPanel(tab);
        tabPage.Controls.Add(tabLayoutPanel);

        // Add forms (GroupBoxes) to the layout panel
        foreach (var formObj in tab.Forms)
        {
            var groupBox = CreateFormGroupBox(formObj);
            tabLayoutPanel.Controls.Add(groupBox);
        }

        return tabPage;
    }

    private TableLayoutPanel CreateTabLayoutPanel(TabObject tab)
    {
        int formsCount = tab.Forms.Count;
        int neededRows = (int)Math.Ceiling((float)formsCount / (float)MAX_FORMS_PER_PAGE);
        float tabSpacing = CalculateTabSpacing(formsCount);

        // Ensure min/max boundaries
        if (formsCount == 0) formsCount = 1;
        else if (formsCount < MIN_FORMS_PER_PAGE) formsCount = MIN_FORMS_PER_PAGE;
        else if (formsCount > MAX_FORMS_PER_PAGE) formsCount = MAX_FORMS_PER_PAGE;

        var tabLayoutPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = false,
            ColumnCount = formsCount,
            RowCount = neededRows,
            Padding = new Padding(2),
            Margin = new Padding(1)
        };

        // Column and row styles
        for (int i = 0; i < formsCount; i++)
            tabLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)Math.Floor(tabSpacing)));

        for (int i = 0; i < neededRows; i++)
            tabLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)Math.Floor((decimal)100 / (decimal)neededRows)));

        return tabLayoutPanel;
    }

    private float CalculateTabSpacing(int formsCount)
    {
        if (formsCount <= 0) return 100; // fallback
        return 100f / formsCount;
    }

    private CustomGroupBox CreateFormGroupBox(FormObject formObj)
    {
        // Distinguish between "test" scenario vs. "regular loading"
        bool isTestMode = (formObj.FormName == null);

        var groupBox = new CustomGroupBox
        {
            Text = isTestMode ? "Form" : formObj.FormName,
            Dock = DockStyle.Fill,
            Padding = new Padding(4),
            Margin = new Padding(2),
            Tag = formObj, // Store the FormObject
        };

        // Build layout for the fields
        TableLayoutPanel layoutPanel = isTestMode
            ? BuildTestLayoutPanel(formObj)
            : BuildRegularLayoutPanel(formObj);

        // If "regular loading," populate with fields
        if (!isTestMode && formObj.Fields != null)
            PopulateFields(layoutPanel, formObj.Fields);

        groupBox.Controls.Add(layoutPanel);

        // Filename box + Generate button
        var fileNameBox = CreateFileNameBox(formObj);
        var generateButton = CreateGenerateButton();
        groupBox.Controls.Add(fileNameBox);
        groupBox.Controls.Add(generateButton);

        return groupBox;
    }

    private TableLayoutPanel BuildTestLayoutPanel(FormObject formObj)
    {
        // Just for demonstration forms (FormName == null)
        var layout = new TableLayoutPanel
        {
            AutoSize = true,
            AutoScroll = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            Dock = DockStyle.Fill
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        return layout;
    }

    private TableLayoutPanel BuildRegularLayoutPanel(FormObject formObj)
    {
        // Attempt to locate the PDF file if needed
        if (!File.Exists(Path.Combine(formObj.Path ?? "", formObj.FileName ?? "")))
        {
            Console.WriteLine(
                $"Unable to locate file: {formObj.FileName} for form name {formObj.FormName} | Please contact software developer.");
        }

        var layout = new TableLayoutPanel
        {
            AutoScroll = true,
            ColumnCount = 3,
            Dock = DockStyle.Fill,
            Name = "layoutPanel",
            Padding = new Padding(10, 0, 0, 0)
            //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            //AutoScrollMargin = new Size(10, 20)
        };
        // Enable auto-scrolling for the TableLayoutPanel

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        
        return layout;
    }

    private void PopulateFields(TableLayoutPanel layout, List<InputField> fields)
    {
        int row = 0;
        foreach (var field in fields)
        {
            try
            {
                //if (field.Type != "ComboBox")
                //layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                var label = CreateFieldLabel(field, layout, row);
                var control = ControlFactory.CreateControlFromJson(field);
                SetupFieldControl(field, control);

                // If there's a description, add the info label in the middle column
                if (!string.IsNullOrEmpty(field.Description))
                {
                    var infoLabel = CreateInfoLabel(field.Description);
                    layout.Controls.Add(infoLabel, 1, row);
                }

                layout.Controls.Add(label, 0, row);
                layout.Controls.Add(control, 2, row);


                // Special handling for "FormFiller" sub-fields
                if (control is ComboBox combo && field.ActionType == "FormFiller" && field.SubFields != null)
                {
                    AddSubFieldsToggle(layout, field, ref row);
                    AddFormFillerDataBindings(combo, field, layout);
                    AddSubFields(layout, field, ref row);
                }
                row++;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating control: {ex.Message}");
            }
        }
    }

    private Label CreateFieldLabel(InputField field, TableLayoutPanel layout, int row)
    {
        var label = new Label
        {
            Text = field.Label,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            Margin = new Padding(ELEMENT_PADDING)
        };
        label.Click += (o, e) => { if (this.debug) new RelocatorForm(field, this.models).ShowDialog(); };

        // Fill in default text if empty
        if (string.IsNullOrEmpty(field.Text))
            field.Text = debug ? field.DebugPlaceholder : field.DefaultText;

        return label;
    }

    private void SetupFieldControl(InputField field, Control control)
    {
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        control.Margin = new Padding(ELEMENT_PADDING);
        // CheckBox
        if (control is CheckBox cb)
        {
            cb.Text = field.DefaultText;
            cb.Tag = field.ActionType;
            cb.DataBindings.Add("Checked", field, nameof(field.Checked), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        // TextBox
        if (control is TextBox tb)
        {
            if (string.IsNullOrEmpty(field.DebugPlaceholder))
                field.Text = field.DefaultText;
            else
                field.Text = debug ? field.DebugPlaceholder : field.DefaultText;
            tb.DataBindings.Add("Text", field, nameof(field.Text), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        // ComboBox (Selector)
        if (control is ComboBox comb && field.ActionType == "Selector")
        {
            field.SelectedItem = new ComboBoxItem { Label = "אחר", Locations = field.Locations, Text = "אחר" };
            field.Text = field.DefaultText ?? "אחר";
            comb.DataBindings.Add("Text", field, nameof(field.Text), false, DataSourceUpdateMode.OnPropertyChanged);
            comb.TextChanged += (sender, args) => OnComboBoxTextChanged(field, comb);
            comb.SelectedIndexChanged += (sender, args) => OnComboBoxSelectedIndexChanged(field, comb);
        }
    }

    private Label CreateInfoLabel(string description)
    {
        var infoLabel = new Label
        {
            Text = "\u2139",
            Font = new Font("Arial", 12),
            ForeColor = System.Drawing.Color.Blue,
            AutoSize = true,
            Dock = DockStyle.Left,
            Cursor = Cursors.Hand,
            Tag = description
        };
        infoLabel.Click += (o, e) =>
        {
            if (o is Control c)
            {
                MessageBox.Show(c.Tag?.ToString(), "מידע על שדה", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        };
        return infoLabel;
    }

    private void OnComboBoxTextChanged(InputField field, ComboBox comb)
    {
        if (!string.IsNullOrEmpty(comb.Text) && comb.SelectedIndex == -1)
        {
            var tempItem = new ComboBoxItem
            {
                Label = comb.Text,
                Text = comb.Text,
                Locations = field.Locations
            };
            field.SelectedItem = tempItem;
            field.Text = comb.Text;
        }
    }

    private void OnComboBoxSelectedIndexChanged(InputField field, ComboBox comb)
    {
        if (comb.SelectedItem is ComboBoxItem selectedItem)
        {
            field.SelectedItem = selectedItem;
            field.Text = selectedItem.Text;
        }
    }

    private void AddSubFieldsToggle(TableLayoutPanel layout, InputField field, ref int row)
    {
        // Add the toggle button in the middle column (same row as the ComboBox)
        var toggleView = new Label
        {
            Text = "\u2193",
            Font = new Font("Arial", 12),
            ForeColor = System.Drawing.Color.Blue,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            AutoSize = true,
            Cursor = Cursors.Hand,
            //BorderStyle = BorderStyle.Fixed3D
        };
        // Place the toggle label in column=1, row = combo's row-1
        layout.Controls.Add(toggleView, 1, row);
        toggleView.Click += (o, e) => ToggleSubFieldsVisibility(field, layout, toggleView);
    }

    private void ToggleSubFieldsVisibility(InputField field, TableLayoutPanel layout, Label toggleView)
    {
        layout.SuspendLayout();
        foreach (Control element in layout.Controls)
        {
            if (element.Tag is object[] arr && arr[0] == field)
            {
                element.Visible = !element.Visible;
                toggleView.Text = element.Visible ? "\u2191" : "\u2193";
            }
        }
        layout.ResumeLayout();
        layout.Parent.PerformLayout();
        layout.PerformLayout();

    }

    private void AddFormFillerDataBindings(ComboBox combo, InputField field, TableLayoutPanel layout)
    {
        // Load data from Personnel/Clients
        if (field.Bank == "Personnel")
        {
            var bindingSource = new BindingSource { DataSource = this.personnel.PersonList };
            combo.DataSource = bindingSource;
            
        }
        else if (field.Bank == "Clients")
        {
            var bindingSource = new BindingSource { DataSource = this.clients.Clients };
            combo.DataSource = bindingSource;
        }
        combo.SelectedIndexChanged += (sender, args) =>
        {
            if (combo.SelectedIndex > -1)
                FillFormWithSelectedItem(field, combo, layout);
        };
    }

    private void FillFormWithSelectedItem(InputField field, ComboBox combo, TableLayoutPanel layout)
    {
        foreach (Control element in layout.Controls)
        {
            if (element.Tag is object[] arr && arr[0] == field)
            {
                layout.SuspendLayout();
                // Re-populate text boxes with the selected Person/Client data
                if (element is TextBox tt && arr[1] != null)
                {
                    bool originalVisibility = tt.Visible;
                    tt.Visible = true;

                    Person.DataType featureType = (Person.DataType)Enum.Parse(typeof(Person.DataType), arr[1].ToString());
                    if (combo.SelectedItem is Person p)
                        AssignPersonData(featureType, tt, p);
                    else if (combo.SelectedItem is Client c)
                        AssignClientData(featureType, tt, c);

                    tt.DataBindings["Text"].WriteValue();
                    tt.Visible = false;
                    tt.Visible = originalVisibility;
                }
                else if (element is Label lbl)
                {
                    bool originalVisibility = lbl.Visible;
                    lbl.Visible = true;
                    lbl.Visible = false;
                    lbl.Visible = originalVisibility;
                }
                layout.ResumeLayout();
            }
        }
    }

    private void AssignPersonData(Person.DataType featureType, TextBox tt, Person person)
    {
        switch (featureType)
        {
            case Person.DataType.Name: tt.Text = person.Name; break;
            case Person.DataType.ID: tt.Text = person.ID; break;
            case Person.DataType.Phone: tt.Text = person.Phone; break;
            case Person.DataType.LicenseType: tt.Text = person.LicenseType; break;
            case Person.DataType.LicenseNumber: tt.Text = person.LicenseNumber; break;
        }
    }

    private void AssignClientData(Person.DataType featureType, TextBox tt, Client client)
    {
        switch (featureType)
        {
            case Person.DataType.Name: tt.Text = client.Name; break;
            case Person.DataType.ID: tt.Text = client.ID; break;
            case Person.DataType.HetPei: tt.Text = client.HetPei; break;
            case Person.DataType.EmailAddress: tt.Text = client.EmailAddress; break;
            case Person.DataType.Phone: tt.Text = client.Phone; break;
        }
    }

    private void AddSubFields(TableLayoutPanel layout, InputField field, ref int row)
    {
        foreach (var subField in field.SubFields)
        {

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var subLabel = CreateSubFieldLabel(subField, field);
            var subControl = CreateSubFieldControl(subField, field);
            row++;

            layout.Controls.Add(subLabel, 0, row);
            layout.Controls.Add(subControl, 2, row);
        }
    }

    private Label CreateSubFieldLabel(InputField subField, InputField parentField)
    {
        var subLabel = new Label
        {
            Text = subField.Label,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            Dock = DockStyle.Top,
            Margin = new Padding(ELEMENT_PADDING),
            Tag = new object[] { parentField, null },
            Visible = false
        };
        subLabel.Click += (o, e) => { if (this.debug) new RelocatorForm(subField, this.models).ShowDialog(); };
        if (string.IsNullOrEmpty(subField.Text))
        {
            subField.Text = debug ? subField.DebugPlaceholder : subField.DefaultText;
        }
        return subLabel;
    }

    private Control CreateSubFieldControl(InputField subField, InputField parentField)
    {
        Control subControl = ControlFactory.CreateControlFromJson(subField);
        subControl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        subControl.Dock = DockStyle.Top;
        subControl.Tag = new object[] { parentField, subField.DataType };
        subControl.Visible = false;
        subControl.Margin = new Padding(ELEMENT_PADDING);

        if (subControl is CheckBox subCB)
        {
            subCB.Tag = subField.ActionType;
            subCB.DataBindings.Add("Checked", subField, nameof(subField.Checked), false, DataSourceUpdateMode.OnPropertyChanged);
        }
        else if (subControl is TextBox subTB)
        {
            subTB.DataBindings.Add("Text", subField, nameof(subField.Text), false, DataSourceUpdateMode.OnPropertyChanged);
        }
        return subControl;
    }

    private TableLayoutPanel CreateFileNameBox(FormObject formObj)
    {
        var fileNameBox = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            RowCount = 1,
            Dock = DockStyle.Bottom,
            Name = "filenameLayout"
        };
        fileNameBox.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        fileNameBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        fileNameBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        var fileNameLabel = new Label
        {
            Text = "שם קובץ:",
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor = AnchorStyles.Left,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 0, 4)
        };

        var fileNameTextBox = new TextBox
        {
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            Margin = new Padding(0, 0, 0, 4),
            Name = "fileNameTextBox",
            Dock = DockStyle.Bottom,
            Tag = "FileName",
            Text = formObj.FormName?.Replace(" ", "_") + "_"
        };
        fileNameTextBox.KeyPress += (s, e) =>
        {
            char[] invalidChars = { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            if (Array.Exists(invalidChars, c => c == e.KeyChar) ||
                (char.IsControl(e.KeyChar) && e.KeyChar != '\b'))
            {
                e.Handled = true; // block invalid chars
            }
        };

        // Debug placeholder
        if (this.debug)
            fileNameTextBox.Text = "aOut_" + formObj.FileName;

        fileNameBox.Controls.Add(fileNameLabel, 0, 0);
        fileNameBox.Controls.Add(fileNameTextBox, 1, 0);

        return fileNameBox;
    }

    private Button CreateGenerateButton()
    {
        var generateButton = new Button
        {
            Text = "הפק טופס",
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Bottom
        };
        generateButton.Click += GenerateButton_Click;
        return generateButton;
    }


    public void GenerateButton_Click(object sender, EventArgs e)
    {
        if (sender is not Button clickedButton)
            throw new TypeAccessException("Sender is not a button. check logic.");

        if (clickedButton.Parent is not GroupBox parentGroupBox)
            throw new NullReferenceException($"Parent of {clickedButton.Name} is not a GroupBox.");

        if (parentGroupBox.Tag is not FormObject formObject)
            throw new MissingFieldException("No Target FormObject given in GroupBox Tag property.");

        if (!Directory.Exists(this.globalSettings.SavePath))
        {
            MessageBox.Show("תיקייה לשמירה לא נמצאה. אנא בחר תיקייה לשמור בה את הקובץ (קובץ > שמור בתיקייה).", "אנא בחר תיקייה", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
            Console.WriteLine($"Failed to find path: {this.globalSettings.SavePath}");
            return;
        }

        string newFilename = ((TextBox)parentGroupBox.Controls.Find("fileNameTextBox", true).First()).Text;

        if (string.IsNullOrEmpty(newFilename))
        {
            MessageBox.Show("אנא ספק שם קובץ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        string pattern = @"^(?!^(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])$)[^<>:""/\\|?*\x00-\x1F]+(?<!\.)$";
        if (!Regex.IsMatch(newFilename, pattern, RegexOptions.IgnoreCase))
        {
            MessageBox.Show("שם קובץ לא תקין", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        if (!newFilename.EndsWith(".pdf"))
        {
            newFilename += ".pdf";
        }
        bool standard = true;

        if (File.Exists(Path.Combine(this.globalSettings.SavePath, newFilename)) && !this.debug)
        {
            DialogResult dr = MessageBox.Show($"קובץ בשם {newFilename} כבר קיים בתיקיית השמירה. האם ברצונך לדרוס קובץ זה?", "אזהרה", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Cancel) { return; }
        }
        foreach (CheckBox checkbox in (parentGroupBox.Controls.Find("layoutPanel", false).First()).Controls.OfType<CheckBox>()) // multiple form versions.
        {
            if (checkbox.Checked && checkbox.Tag != null)
            {
                string? actionType = checkbox.Tag?.ToString()?.Split(',')[0];
                if (actionType == "NewForm" && checkbox.Checked)
                {
                    string inputFormname = checkbox.Tag?.ToString()?.Split(',')[1].Trim(); // Get the input filename
                    string outputName = newFilename.Replace(".pdf", "") + "_" + checkbox.Text.Replace(" ", "_") + ".pdf";  // Produce output file
                    standard = false;

                    formObject.FillSpecialForm(outputName, globalSettings.SavePath, inputFormname, globalSettings.InputPath, new DeviceRgb(this.mainColor.R, this.mainColor.G, this.mainColor.B));
                    Console.WriteLine($"Form has been filled and saved at {Path.Combine(globalSettings.SavePath, outputName)}");
                }
            }
        }
        if (standard)
        {
            formObject.FillForm(newFilename, globalSettings.SavePath, globalSettings.InputPath, new DeviceRgb(this.mainColor.R, this.mainColor.G, this.mainColor.B));
            Console.WriteLine($"Form has been filled and saved at {Path.Combine(globalSettings.SavePath, newFilename)}");
            if (globalSettings.LaunchFileAtGeneration)
            {
                Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = Path.Combine(globalSettings.SavePath, newFilename),
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }
    }

    private MenuStrip GenerateMenuStrip()
    {
        MenuStrip menuStrip = new MenuStrip();
        ToolStripMenuItem fileMenu = new ToolStripMenuItem("קובץ");
        ToolStripMenuItem chooseSaveFolder = new ToolStripMenuItem("שמור בתיקייה");
        chooseSaveFolder.Click += (s, e) => SaveFolderDialoge();

        ToolStripMenuItem colorPallete = new ToolStripMenuItem("בחר צבע");
        colorPallete.Click += (s, e) =>
        {
            if (mainColorDialog.ShowDialog() == DialogResult.OK)
            {
                this.mainColor = mainColorDialog.Color;
            }
        };
        ToolStripMenuItem openFolder = new ToolStripMenuItem("פתח תיקייה מכילה");
        openFolder.Click += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
        {
            FileName = globalSettings.SavePath,
            UseShellExecute = true,
            Verb = "open"
        });

        ToolStripMenuItem about = new ToolStripMenuItem("אודות");
        about.Click += (o, e) => { new AboutBox().ShowDialog(); };

        ToolStripMenuItem edit = new ToolStripMenuItem("עריכה");
        ToolStripMenuItem editPersonnel = new ToolStripMenuItem("ערוך מאגר אנשי מקצוע");
        ToolStripMenuItem editClients = new ToolStripMenuItem("ערוך מאגר לקוחות");
        edit.DropDownItems.Add(editPersonnel);
        edit.DropDownItems.Add(editClients);
        editPersonnel.Click += (o, e) => { var f = new PersonnelForm(this.personnel, this.clients, 0); f.FormClosing += (s, e) => { this.PerformLayout(); }; f.Show(); };
        editClients.Click += (o, e) =>
        {
            var f = new PersonnelForm(this.personnel, this.clients, 1); f.FormClosing += (s, e) => { this.PerformLayout(); };
            f.Show();
        };
        ToolStripMenuItem openFileAfterGeneration = new ToolStripMenuItem("פתח קובץ לאחר הפקה");
        openFileAfterGeneration.Checked = this.globalSettings.LaunchFileAtGeneration;
        openFileAfterGeneration.Click += (o, e) => { this.globalSettings.LaunchFileAtGeneration = !this.globalSettings.LaunchFileAtGeneration; openFileAfterGeneration.Checked = this.globalSettings.LaunchFileAtGeneration; Console.WriteLine($"Open file after generation status: {this.globalSettings.LaunchFileAtGeneration}"); };

        ToolStripMenuItem debugMode = new ToolStripMenuItem("מצב פיתוח");
        debugMode.Checked = this.globalSettings.Debug;
        debugMode.Click += (o, e) => { this.globalSettings.Debug = !this.globalSettings.Debug; debugMode.Checked = this.globalSettings.Debug; Config.UpdateSettings(this.globalSettings); MessageBox.Show("Please restart the program for changes to take effect."); };


        fileMenu.DropDownItems.Add(chooseSaveFolder);
        fileMenu.DropDownItems.Add(openFolder);
        fileMenu.DropDownItems.Add(openFileAfterGeneration);
        fileMenu.DropDownItems.Add(colorPallete);
        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
        fileMenu.DropDownItems.Add(toolStripSeparator1);
        fileMenu.DropDownItems.Add(debugMode);
        menuStrip.Items.Add(fileMenu);
        menuStrip.Items.Add(edit);
        menuStrip.Items.Add(about);
        this.MainMenuStrip = menuStrip;
        return menuStrip;
    }

    private FormObject FillDataStructure(TableLayoutControlCollection controls, string formName, string tabName)
    {
        TabObject tabObject = null;
        foreach (TabObject tab in models.Tabs)
        {
            if (tab.TabName == tabName)
            {
                tabObject = tab;
                break;
            }
        }
        if (tabObject == null)
        {
            throw new NullReferenceException("could not find matching tab in settings");
        }
        FormObject formObject = null;
        foreach (FormObject form in tabObject.Forms)
        {
            if (form.FormName == formName)
            {
                formObject = form;
                break;
            }
        }
        if (formObject == null) throw new NullReferenceException("could not find matching form name in settings"); ;

        // Iterate through the controls and update the corresponding InputField in the data structure
        foreach (Control control in controls)
        {
            if (control is TextBox textBox)
            {
                var matchingField = formObject.Fields?.Find(field => field.Name == control.Name);
                if (matchingField != null)
                {
                    matchingField.Text = textBox.Text;
                    //Console.WriteLine($"Found and filled the {matchingField} object with {control.Text}");
                }
            }
            else if (control is CheckBox checkBox)
            {
                var matchingField = formObject.Fields?.Find(field => field.Name == control.Name);
                if (matchingField != null)
                {
                    matchingField.Text = "\u00D7";
                    matchingField.Checked = checkBox.Checked;
                    //Console.WriteLine($"Found and filled the {matchingField} object with {checkBox.Checked.ToString()}");
                }
            }
            else if (control is ComboBox comboBox)
            {
                var matchingField = formObject.Fields?.Find(field => field.Name == control.Name);
                if (matchingField != null)
                {
                    if (comboBox.SelectedIndex == -1)
                    {
                        matchingField.SelectedItem = new ComboBoxItem { Label = comboBox.Text, Locations = matchingField.Locations, Text = comboBox.Text };
                    }
                    else
                    {
                        matchingField.SelectedItem = (ComboBoxItem)comboBox.SelectedItem;
                        matchingField.Text = ((ComboBoxItem)comboBox.SelectedItem)?.Text;
                    }
                    //Console.WriteLine($"Found and filled the {matchingField} object with {comboBox.SelectedItem.ToString()}");
                }
            }
        }
        return formObject;

    }
    private void SaveFolderDialoge()
    {
        using (FolderBrowserDialog fbd = new FolderBrowserDialog())
        {
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                this.globalSettings.SavePath = fbd.SelectedPath;
                Config.UpdateSettings(this.globalSettings);
                Console.WriteLine($"Files will be saved at: {fbd.SelectedPath}");
            }
        }
    }
    private void ToggleHighContrast(bool enableHighContrast)
    {
        if (enableHighContrast)
        {
            //this.BackColor = System.Drawing.Color.Black;
            //this.ForeColor = System.Drawing.Color.White;

            foreach (Control control in this.Controls)
            {
                //control.BackColor = System.Drawing.Color.Black;
                //control.ForeColor = System.Drawing.Color.White;
                control.Font = new Font(control.Font.FontFamily, control.Font.Size, FontStyle.Bold);
            }
        }
        else
        {
            this.BackColor = SystemColors.Control;
            this.ForeColor = SystemColors.ControlText;

            foreach (Control control in this.Controls)
            {
                control.BackColor = SystemColors.Control;
                control.ForeColor = SystemColors.ControlText;
            }
        }
    }

    public class CustomGroupBox : GroupBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            // Use a custom bold font with an explicitly set size
            float fontSize = this.Font.Size + 0.5f; // Preserve the GroupBox font size
            using (Pen pen = new Pen(System.Drawing.Color.Black, 1.5f)) // Customize color and thickness
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                Rectangle borderRectangle = new Rectangle(
                    this.ClientRectangle.X,
                    this.ClientRectangle.Y + 7,
                    this.ClientRectangle.Width - 1,
                    this.ClientRectangle.Height - 8
                );
                e.Graphics.DrawRectangle(pen, borderRectangle);
            }
            using (Font boldFont = new Font(this.Font.FontFamily, fontSize, FontStyle.Bold))
            {
                // Measure the size of the title text
                SizeF textSize = e.Graphics.MeasureString(this.Text, boldFont);

                // Determine text position based on RightToLeft property
                Point textLocation;
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    // Align text to the right side
                    textLocation = new Point(
                        this.Width - (int)textSize.Width - 10, // 10px padding from the right
                        0
                    );
                }
                else
                {
                    // Align text to the left side
                    textLocation = new Point(10, 0); // 10px padding from the left
                }

                // Create a rectangle for clearing the background behind the text
                Rectangle textRect = new Rectangle(
                    textLocation.X, textLocation.Y,
                    (int)textSize.Width + 2, (int)textSize.Height
                );

                // Clear the background behind the text
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);

                // Draw the title text
                e.Graphics.DrawString(this.Text, boldFont, Brushes.Black, textLocation);
            }

        }
    }

    public void CalculateWindowSize()
    {
        if (this.models == null) { return; }
        float windowHeight = console.Height;
        windowHeight += this.MainMenuStrip.Height;
        // find max count of fields and forms
        int maxFields = 0;
        int maxForms = 0;
        foreach (var tab in models.Tabs)
        {
            if (tab.Forms.Count > maxForms) { maxForms = tab.Forms.Count; }
            foreach (var form in tab.Forms)
            {
                if (form.Fields == null) { continue; }
                if (form.Fields.Count > maxFields) { maxFields = form.Fields.Count; }
            }
        }
        float neededRows = (float)Math.Sqrt(Math.Ceiling((float)maxForms / (float)MAX_FORMS_PER_PAGE));
        if (maxForms > MAX_FORMS_PER_PAGE) { maxForms = MAX_FORMS_PER_PAGE; }
        this.Width = maxForms * MIN_FORM_WIDTH;
        this.MinimumSize = new System.Drawing.Size(this.Width, this.MinimumSize.Height);
        windowHeight += (int)Math.Ceiling((double)maxFields * (double)SPACE_PER_INPUT * (double)neededRows);
        this.Height = (int)windowHeight;
    }
}

