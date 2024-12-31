using iText.Kernel.Colors;
using iText.Kernel.Utils.Objectpathitems;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
    private Personnel personnel;
    private GlobalSettings globalSettings;
    private Models models;
    private bool debug;
    private const int MAX_FORMS_PER_PAGE = 5;
    private const int MIN_FORMS_PER_PAGE = 3;
    private const int MIN_FORM_WIDTH = 400;
    private const int SPACE_PER_INPUT = 65;
    public Main()
    {
        InitializeComponent();
        try
        {
            this.globalSettings = Config.PullSettings();
            this.models = Config.PullModels();
            this.personnel = Config.PullPersonnel();
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

        MenuStrip mainMenuStrip = GenerateMenuStrip();
        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 3
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // For MenuStrip
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // For TabControl
        mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // For Console


        TabControl tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            RightToLeftLayout = true,
        };


        foreach (var tab in this.models.Tabs)
        {
            TabPage tabPage = new TabPage
            {
                Margin = new Padding(25),
                Text = tab.TabName,
                //AutoScroll = false,
            };
            int formsCount = tab.Forms.Count;
            int neededRows = (int)Math.Ceiling((float)formsCount / (float)MAX_FORMS_PER_PAGE);
            float tabSpacing = 0;
            if (formsCount == 0) {; }
            else if (formsCount <= MIN_FORMS_PER_PAGE)
            {
                formsCount = MIN_FORMS_PER_PAGE;
                tabSpacing = 100 / formsCount;
            }
            else if (formsCount >= MAX_FORMS_PER_PAGE)
            {
                formsCount = MAX_FORMS_PER_PAGE;
                tabSpacing = 100 / formsCount;
            }
            else
            {
                tabSpacing = 100 / formsCount;
            }



            TableLayoutPanel tabLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                //AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = formsCount,
                RowCount = neededRows,
                Padding = new Padding(2),
                Margin = new Padding(1),
            };


            for (int i = 0; i < formsCount; i++)
            {
                tabLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, ((float)Math.Floor(tabSpacing))));
            }
            for (int i = 0; i < neededRows; i++)
            {
                tabLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)Math.Floor((decimal)100 / (decimal)neededRows)));
            }


            int formNum = 1;
            foreach (var group in tab.Forms)
            {
                TableLayoutPanel layout;
                CustomGroupBox groupBox;
                int row = 0;
                if (group.FormName == null) // <<<<<<< FOR TESTING PURPOUSE
                {
                    groupBox = new CustomGroupBox
                    {
                        Text = (formNum++).ToString(),
                        Dock = DockStyle.Fill,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowOnly,
                        Size = new Size(300, 700),
                        Padding = new Padding(4),
                        Margin = new Padding(2),
                        Tag = group // FOR TESTING
                    };

                    layout = new TableLayoutPanel // FOR TESTING
                    {
                        AutoSize = true,
                        AutoScroll = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        ColumnCount = 2,
                        Dock = DockStyle.Fill,
                        // FOR TESTING
                    };

                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

                }
                else // REGULAR LOADING CASE
                {
                    if (!File.Exists(Path.Combine(group.Path, group.FileName))) { Console.WriteLine($"Unable to locate file: {group.FileName} for form name {group.FormName} | Please contact software developer."); }
                    groupBox = new CustomGroupBox
                    {
                        Text = group.FormName,
                        Dock = DockStyle.Fill,
                        //AutoSize = false,
                        //AutoSizeMode = AutoSizeMode.GrowOnly,
                        //Size = new Size(300, 700),
                        Padding = new Padding(4),
                        Margin = new Padding(2),
                        Tag = group//Path.Combine(group.Path, group.FileName), // Tag holds the path to the file
                    };

                    layout = new TableLayoutPanel
                    {
                        AutoScroll = true,
                        
                        ColumnCount = 3,
                        Dock = DockStyle.Fill,
                        //Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom,
                        Name = "layoutPanel",
                        //CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
                    };

                    layout.AutoScrollMinSize = new Size(0, layout.GetPreferredSize(Size.Empty).Height);

                    //layout.AutoScrollMinSize = layout.GetPreferredSize(Size.Empty);

                    layout.Click += (o, e) => { Console.WriteLine(layout.Size); };
                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 35));
                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));


                    if (group.Fields != null)
                    {
                        foreach (var field in group.Fields)
                        {
                            try
                            {
                                if (field.Type != "ComboBox")
                                {
                                    layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
                                }
                                //layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                                //layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
                                Label label = new Label
                                {
                                    Text = field.Label,
                                    AutoSize = true,
                                    TextAlign = ContentAlignment.MiddleLeft,
                                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                                    Dock = DockStyle.Top,
                                    
                                    
                                };
                                label.Click += (o, e) => new RelocatorForm(field, this.models).ShowDialog();

                                if (string.IsNullOrEmpty(field.Text))
                                    field.Text = debug ? field.DebugPlaceholder : field.DefaultText;

                                Control control = ControlFactory.CreateControlFromJson(field);
                                control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                                control.Dock = DockStyle.Top;
                                if (!string.IsNullOrEmpty(field.Description))
                                {
                                    Label infoLabel = new Label
                                    {
                                        Text = "\u2139", // Unicode for "Information" symbol
                                        Font = new Font("Arial", 12), // Adjust font and size
                                        ForeColor = System.Drawing.Color.Blue, // Color for visibility
                                        AutoSize = true,
                                        Dock = DockStyle.Left,
                                        Cursor = Cursors.Hand, // Optional: Hand cursor
                                        Tag = field.Description
                                    };
                                    infoLabel.Click += (o, e) =>
                                    {
                                        if (o is Control c)
                                        {
                                            MessageBox.Show(c.Tag?.ToString(), "מידע על שדה", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    };
                                    layout.Controls.Add(infoLabel, 1, row);
                                } // Information Button

                                if (control is CheckBox cb)
                                {
                                    cb.Text = field.DefaultText;
                                    cb.Tag = field.ActionType;
                                    cb.DataBindings.Add("Checked", field, nameof(field.Checked), false, DataSourceUpdateMode.OnPropertyChanged);
                                }

                                if (control is TextBox tb)
                                    tb.DataBindings.Add("Text", field, nameof(field.Text), false, DataSourceUpdateMode.OnPropertyChanged);

                                if (control is ComboBox comb && field.ActionType == "Selector")
                                {
                                    field.SelectedItem = new ComboBoxItem { Label = "אחר", Locations = field.Locations, Text = "אחר" };
                                    field.Text = field.DefaultText ?? "אחר";

                                    comb.DataBindings.Add("Text", field, nameof(field.Text), false, DataSourceUpdateMode.OnPropertyChanged);

                                    comb.TextChanged += (sender, args) =>
                                    {
                                        if (!string.IsNullOrEmpty(comb.Text) && comb.SelectedIndex == -1)
                                        {
                                            var tempItem = new ComboBoxItem
                                            {
                                                Label = comb.Text,
                                                Text = comb.Text,
                                                Locations = field.Locations // Optional: Retain locations if needed
                                            };

                                            field.SelectedItem = tempItem;
                                            field.Text = comb.Text;
                                        }
                                    };

                                    comb.SelectedIndexChanged += (sender, args) =>
                                    {
                                        if (comb.SelectedItem is ComboBoxItem selectedItem)
                                        {
                                            field.SelectedItem = selectedItem;
                                            field.Text = selectedItem.Text;
                                        }
                                    };
                                }
                                
                                layout.Controls.Add(label, 0, row); // Add label in the first column
                                layout.Controls.Add(control, 2, row); // Add control in the third column
                                row++;

                                if (control is ComboBox combo && field.ActionType == "FormFiller" && field.SubFields != null)
                                {
                                    layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                                    Label toggleView = new Label
                                    {
                                        Text = "\u2193",
                                        Font = new Font("Arial", 12), // Adjust font and size
                                        ForeColor = System.Drawing.Color.Blue, // Color for visibility
                                        //AutoSize = true,
                                        //Dock = DockStyle.Left,
                                        Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                                        Height = 28,
                                        Cursor = Cursors.Hand, // Optional: Hand cursor
                                        BorderStyle = BorderStyle.Fixed3D
                                    };
                                    toggleView.Click += (o, e) =>
                                    {
                                        foreach (Control element in layout.Controls)
                                        {
                                            object[] arr;
                                            try
                                            {
                                                arr = ((object[])element.Tag);
                                                if (arr == null) { continue; }
                                            }
                                            catch { continue; }
                                            if (arr[0] == field)
                                            {


                                                element.Visible = !element.Visible;
                                                if (element.Visible)
                                                {
                                                    toggleView.Text = "\u2191";
                                                    //layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
                                                }
                                                else
                                                {
                                                    toggleView.Text = "\u2193";
                                                    //layout.RowStyles.Remove(new RowStyle(SizeType.Absolute, 35));
                                                }
                                            }
                                        }
                                    };
                                    layout.Controls.Add(toggleView, 1, row-1);


                                    combo.Items.AddRange(this.personnel.PersonList.ToArray());
                                    combo.SelectedIndexChanged += (sender, args) =>
                                    {
                                        if (combo.SelectedIndex > -1)
                                        {
                                            foreach (Control element in layout.Controls)
                                            {
                                                object[] arr;
                                                try
                                                {
                                                    arr = ((object[])element.Tag);
                                                    if (arr == null) { continue; }
                                                    if (arr[1] == null) { continue; }
                                                }
                                                catch { continue; }
                                                if (arr[0] == field && element is TextBox tt)
                                                {
                                                    bool state = tt.Visible;
                                                    tt.Visible = true;
                                                    Person.PersonDataType featureType = (Person.PersonDataType)Enum.Parse(typeof(Person.PersonDataType), arr[1].ToString());
                                                    switch (featureType)
                                                    {
                                                        case Person.PersonDataType.Name:
                                                            tt.Text = ((Person)combo.SelectedItem).Name;
                                                            break;
                                                        case Person.PersonDataType.ID:
                                                            tt.Text = ((Person)combo.SelectedItem).ID;
                                                            break;
                                                        case Person.PersonDataType.LicenseType:
                                                            tt.Text = ((Person)combo.SelectedItem).LicenseType;
                                                            break;
                                                        case Person.PersonDataType.LicenseNumber:
                                                            tt.Text = ((Person)combo.SelectedItem).LicenseNumber;
                                                            break;
                                                        case Person.PersonDataType.Phone:
                                                            tt.Text = ((Person)combo.SelectedItem).Phone;
                                                            break;
                                                    }
                                                    tt.DataBindings["Text"].WriteValue();
                                                    tt.Visible = false;
                                                    tt.Visible = state;
                                                }
                                                if (arr[0] == field && element is Label l)
                                                {
                                                    bool state = l.Visible;
                                                    l.Visible = true;
                                                    l.Visible = false;
                                                    l.Visible = state;  
                                                }
                                            }
                                        }
                                    };
                                    int subFieldsCount = field.SubFields.Count;
                                    int counter = 0;
                                    foreach (InputField subField in field.SubFields)
                                    {
                                        if (subFieldsCount - counter > 1)
                                            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                                        else
                                            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
                                        Label subLabel = new Label
                                        {
                                            Text = subField.Label,
                                            AutoSize = true,
                                            TextAlign = ContentAlignment.MiddleLeft,
                                            Anchor = AnchorStyles.Left | AnchorStyles.Right,
                                            Dock = DockStyle.Top,
                                            Tag = new object[] { field, null },
                                        };
                                        subLabel.Visible = false;
                                        subLabel.Click += (o, e) => new RelocatorForm(subField, this.models).ShowDialog();

                                        if (string.IsNullOrEmpty(subField.Text))
                                            subField.Text = debug ? subField.DebugPlaceholder : subField.DefaultText;


                                        Control subControl = ControlFactory.CreateControlFromJson(subField);
                                        subControl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                                        subControl.Dock = DockStyle.Top;
                                        subControl.Tag = new object[] { field, subField.DataType };
                                        subControl.Visible = false;

                                        if (subControl is CheckBox subCB)
                                        {
                                            subCB.Tag = subField.ActionType;
                                            subCB.DataBindings.Add("Checked", subField, nameof(subField.Checked), false, DataSourceUpdateMode.OnPropertyChanged);
                                        }

                                        if (subControl is TextBox subTB)
                                            subTB.DataBindings.Add("Text", subField, nameof(subField.Text), false, DataSourceUpdateMode.OnPropertyChanged);

                                        layout.Controls.Add(subLabel, 0, row);
                                        layout.Controls.Add(subControl, 2, row);
                                        row++;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error creating control: {ex.Message}");
                            }
                        }
                    }
                }
                TableLayoutPanel fileNameBox = new TableLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    ColumnCount = 2,
                    RowCount = 1,
                    Dock = DockStyle.Bottom,
                    Name = "filenameLayout",

                };
                fileNameBox.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                fileNameBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                fileNameBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

                Label fileNameLabel = new Label
                {
                    Text = "שם קובץ:",
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Anchor = AnchorStyles.Left,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 0, 0, 4)
                };

                TextBox fileNameTextBox = new TextBox
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    Margin = new Padding(0, 0, 0, 4),
                    Name = "fileNameTextBox",
                    Dock = DockStyle.Bottom,
                    Tag = "FileName",
                    Text = group.FormName?.Replace(" ", "_") + "_"
                };
                fileNameTextBox.KeyPress += (s, e) =>
                {
                    char[] invalidChars = { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
                    if (Array.Exists(invalidChars, c => c == e.KeyChar) || char.IsControl(e.KeyChar) && e.KeyChar != '\b')
                        e.Handled = true; // Blocks the input
                }; // Suppress the input if the character is disallowed
                if (this.debug)
                    fileNameTextBox.Text = "aOut_" + group.FileName;


                fileNameBox.Controls.Add(fileNameLabel, 0, 0);
                fileNameBox.Controls.Add(fileNameTextBox, 2, 0);
                row++;
                var generateButton = new Button
                {
                    Text = "הפק טופס",
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Dock = DockStyle.Bottom,

                };

                generateButton.Click += GenerateButton_Click;

                groupBox.Controls.Add(layout);
                groupBox.Controls.Add(fileNameBox);
                groupBox.Controls.Add(generateButton);

                tabLayoutPanel.Controls.Add(groupBox);
            }

            tabPage.Controls.Add(tabLayoutPanel);
            tabControl.TabPages.Add(tabPage);
        }

        this.console = new TextBox
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            ReadOnly = true,
            Multiline = true,
            Height = 75,
            ScrollBars = ScrollBars.Vertical,
            RightToLeft = RightToLeft.No
        };

        mainLayout.Controls.Add(mainMenuStrip, 0, 0);
        mainLayout.Controls.Add(tabControl, 0, 1);
        mainLayout.Controls.Add(this.console, 0, 2);
        this.Controls.Add(mainLayout);
        Console.SetOut(new TextBoxWriter(console));
        CalculateWindowSize();
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

        if (File.Exists(Path.Combine(this.globalSettings.SavePath, newFilename)) && !this.debug) {
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

