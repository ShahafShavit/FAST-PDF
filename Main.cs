using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Windows.Forms;


namespace Auto_UI_Test
{
    public partial class Main : System.Windows.Forms.Form
    {
        private TextBox console;
        private UIConfig config;
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
                this.config = Config.Pull();
            }
            catch (Exception e)
            { MessageBox.Show("Error: " + e.Message); Environment.Exit(1); }
            this.debug = this.config.GeneralSettings.Debug;
            this.Text = "מערכת מילוי טפסים- להט הנדסת חשמל";
            this.RightToLeft = RightToLeft.Yes;

            GenerateUI();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            Console.WriteLine("Initialization of components has been completed.");
            this.FormClosing += (o, e) =>
            {
                Config.Update(config);
                if (debug)
                {
                    string debugFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
                    string solutionFilePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "config.json");

                    File.Copy(debugFilePath, solutionFilePath, true);
                }
            };
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


            foreach (var tab in config.Tabs)
            {
                TabPage tabPage = new TabPage
                {
                    Margin = new Padding(25),
                    Text = tab.TabName,
                    AutoScroll = true,
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
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
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
                    GroupBox groupBox;
                    int row = 0;
                    if (group.FormName == null) // <<<<<<< FOR TESTING PURPOUSE
                    { 
                        groupBox = new GroupBox
                        {
                            Text = (formNum++).ToString(),
                            Dock = DockStyle.Fill,
                            AutoSize = true,
                            AutoSizeMode = AutoSizeMode.GrowOnly,
                            Size = new Size(300, 700),
                            Padding = new Padding(4),
                            Margin = new Padding(2),
                        };

                        layout = new TableLayoutPanel
                        {
                            AutoSize = true,
                            AutoSizeMode = AutoSizeMode.GrowAndShrink,
                            ColumnCount = 2,
                            Dock = DockStyle.Fill,

                        };

                        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

                    }
                    else // REGULAR LOADING CASE
                    {
                        if (!File.Exists(Path.Combine(group.Path, group.FileName))) { Console.WriteLine($"Unable to locate file: {group.FileName} for form name {group.FormName} | Please contact software developer."); }
                        groupBox = new GroupBox
                        {
                            Text = group.FormName,
                            Dock = DockStyle.Fill,
                            AutoSize = true,
                            AutoSizeMode = AutoSizeMode.GrowOnly,
                            Size = new Size(300, 700),
                            Padding = new Padding(4),
                            Margin = new Padding(2),
                            Tag = Path.Combine(group.Path, group.FileName), // Tag holds the path to the file
                        };

                        layout = new TableLayoutPanel
                        {
                            AutoSize = true,
                            AutoSizeMode = AutoSizeMode.GrowAndShrink,
                            ColumnCount = 3,
                            Dock = DockStyle.Fill,

                        };

                        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
                        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
                        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));


                        if (group.Fields != null)
                        {
                            foreach (var field in group.Fields)
                            {
                                try
                                {
                                    Label label = new Label
                                    {
                                        Text = field.Label,
                                        AutoSize = true,
                                        TextAlign = ContentAlignment.MiddleLeft,
                                        Anchor = AnchorStyles.Left,
                                        Dock = DockStyle.Top,
                                        Margin = new Padding(0, 0, 0, 15),
                                    };


                                    Control control = ControlFactory.CreateControlFromJson(field);
                                    control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                                    control.Margin = new Padding(0, 0, 0, 15);
                                    control.Dock = DockStyle.Top;
                                    control.Text = field.DefaultText;
                                    label.Click += (o, e) => new RelocatorForm(field, config).ShowDialog();
                                    if (control is CheckBox cb)
                                    {
                                        cb.Tag = field.ActionType;
                                    }
                                    if (this.debug && string.IsNullOrEmpty(control.Text))
                                    {
                                        control.Text = field.Placeholder;
                                    }
                                    if (!string.IsNullOrEmpty(field.Description))
                                    {
                                        Label infoLabel = new Label
                                        {
                                            Text = "\u2139", // Unicode for "Information" symbol
                                            Font = new Font("Arial", 12), // Adjust font and size
                                            ForeColor = Color.Blue, // Color for visibility
                                            AutoSize = true,
                                            Dock = DockStyle.Left,
                                            Cursor = Cursors.Hand, // Optional: Hand cursor
                                            Tag = field.Description
                                        };
                                        //infoLabel.Click += HelpButton_Click;
                                        infoLabel.Click += (o, e) =>
                                        {
                                            if (o is Control c)
                                            {
                                                MessageBox.Show(c.Tag.ToString(), "מידע על שדה", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        };
                                        layout.Controls.Add(infoLabel, 1, row);
                                    }



                                    layout.Controls.Add(label, 0, row); // Add label in the first column
                                    layout.Controls.Add(control, 2, row); // Add control in the third column
                                    row++;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error creating control: {ex.Message}");
                                }
                            }
                        }
                    }
                    Label fileNameLabel = new Label
                    {
                        Text = "שם קובץ:",
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Anchor = AnchorStyles.Left,
                        Dock = DockStyle.Bottom,
                        Margin = new Padding(0, 0, 0, 4)
                    };

                    TextBox fileNameTextBox = new TextBox
                    {
                        Anchor = AnchorStyles.Left | AnchorStyles.Right,
                        Margin = new Padding(0, 0, 0, 4),
                        Dock = DockStyle.Bottom,
                        Tag = "FileName",
                        Text = group.FormName?.Replace(" ", "_") + "_"
                    };
                    if (this.debug)
                    {
                        fileNameTextBox.Text = "aOut_" + group.FileName;
                    }

                    layout.Controls.Add(fileNameLabel, 0, row);
                    layout.Controls.Add(fileNameTextBox, 2, row);
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
            RedirectConsoleOutput();
            CalculateWindowSize();
        }

        public void HelpButton_Click(object sender, EventArgs e)
        {
            if (sender is Control c)
            {
                MessageBox.Show(c.Tag?.ToString(), "מידע על שדה", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        public void GenerateButton_Click(object sender, EventArgs e)
        {
            if (sender is not Button clickedButton)
                throw new TypeAccessException("Sender is not a button. check logic.");

            if (clickedButton.Parent is not GroupBox parentGroupBox)
                throw new NullReferenceException($"Parent of {clickedButton.Name} is not a GroupBox.");

            if (parentGroupBox.Tag is not string tag || string.IsNullOrEmpty(tag))
                throw new MissingFieldException("No file path given in GroupBox Tag property.");

            if (parentGroupBox.Tag.ToString() is not String originalPath)
                throw new MissingFieldException("No file path given in GroupBox Tag property, 2.");
            if (!Directory.Exists(this.config.GeneralSettings.SavePath))
            {
                MessageBox.Show("אנא בחר תיקייה לשמור בה את הקובץ (קובץ > שמור בתיקייה).", "אנא בחר תיקייה", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
                Console.WriteLine($"Failed to find path: {this.config.GeneralSettings.SavePath}");
                return;
            }
            //if (debug) this.config = Config.Pull();
            
            TableLayoutPanel layoutPanel = null;
            foreach (Control control in parentGroupBox.Controls)
            {
                if (control is TableLayoutPanel tableLayout)
                {
                    layoutPanel = tableLayout;
                    break;
                }
            }

            if (layoutPanel == null)
            {
                MessageBox.Show("TableLayoutPanel not found in GroupBox.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string newFilename = string.Empty;
            foreach (Control control in layoutPanel.Controls)
            {
                if (control is TextBox textBox && textBox.Tag?.ToString() == "FileName")
                {
                    newFilename = textBox.Text;
                    break;
                }
            }
            if (string.IsNullOrEmpty(newFilename))
            {
                MessageBox.Show("Filename not provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!newFilename.EndsWith(".pdf"))
            {
                newFilename += ".pdf";
            }
            bool standard = true;
            foreach (CheckBox checkbox in layoutPanel.Controls.OfType<CheckBox>())
            {
                if (checkbox.Checked && checkbox.Tag != null)
                {
                    string? actionType = checkbox.Tag?.ToString()?.Split(',')[0];
                    if (actionType == "NewForm" && checkbox.Checked)
                    {
                        string inputFormname = checkbox.Tag?.ToString()?.Split(',')[1].Trim(); // Get the input filename
                        string outputName = newFilename.Replace(".pdf", "") + "_" + checkbox.Text.Replace(" ", "_") +".pdf";  // Produce output file

                        standard = false;

                        FormObject fo = FillDataStructure(layoutPanel.Controls, parentGroupBox.Text, clickedButton.Parent.Parent.Parent.Text);
                        fo.FillSpecialForm(outputName, config.GeneralSettings.SavePath, inputFormname, config.GeneralSettings.InputPath);
                        Console.WriteLine($"Form has been filled and saved at {Path.Combine(config.GeneralSettings.SavePath, outputName)}");
                    }
                }
            }
            if (standard)
            {
                FormObject fo = FillDataStructure(layoutPanel.Controls, parentGroupBox.Text, clickedButton.Parent.Parent.Parent.Text);
                fo.FillForm(newFilename, config.GeneralSettings.SavePath, config.GeneralSettings.InputPath);
                Console.WriteLine($"Form has been filled and saved at {Path.Combine(config.GeneralSettings.SavePath, newFilename)}");
                if (config.GeneralSettings.LaunchFileAtGeneration)
                {
                    Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = Path.Combine(config.GeneralSettings.SavePath, newFilename),
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
            }
        }
        private void RedirectConsoleOutput()
        {
            Console.SetOut(new TextBoxWriter(console));
        }
        private MenuStrip GenerateMenuStrip()
        {

            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("קובץ");
            ToolStripMenuItem chooseSaveFolder = new ToolStripMenuItem("שמור בתיקייה");
            chooseSaveFolder.Click += (s, e) => SaveFolderDialoge();
            
            ToolStripMenuItem openFolder = new ToolStripMenuItem("פתח תיקייה מכילה");
            openFolder.Click += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = config.GeneralSettings.SavePath,
                UseShellExecute = true,
                Verb = "open"
            });
            
            ToolStripMenuItem about = new ToolStripMenuItem("אודות");
            about.Click += (o, e) => { new AboutBox().ShowDialog(); };
            
            ToolStripMenuItem openFileAfterGeneration = new ToolStripMenuItem("פתח קובץ לאחר הפקה");
            openFileAfterGeneration.Checked = this.config.GeneralSettings.LaunchFileAtGeneration;
            openFileAfterGeneration.Click += (o, e) => { this.config.GeneralSettings.LaunchFileAtGeneration = !this.config.GeneralSettings.LaunchFileAtGeneration; openFileAfterGeneration.Checked = this.config.GeneralSettings.LaunchFileAtGeneration; Console.WriteLine($"Open file after generation status: {this.config.GeneralSettings.LaunchFileAtGeneration}"); };

            ToolStripMenuItem debugMode = new ToolStripMenuItem("מצב פיתוח");
            debugMode.Checked = this.config.GeneralSettings.Debug;
            debugMode.Click += (o, e) => { this.config.GeneralSettings.Debug = !this.config.GeneralSettings.Debug; debugMode.Checked = this.config.GeneralSettings.Debug; };

            fileMenu.DropDownItems.Add(chooseSaveFolder);
            fileMenu.DropDownItems.Add(openFolder);
            fileMenu.DropDownItems.Add(openFileAfterGeneration);
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
            foreach (TabObject tab in config.Tabs)
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
                bool found = false;
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
                        matchingField.Text = checkBox.Checked.ToString();
                        //Console.WriteLine($"Found and filled the {matchingField} object with {checkBox.Checked.ToString()}");
                    }
                }
                else if (control is ComboBox comboBox)
                {
                    var matchingField = formObject.Fields?.Find(field => field.Name == control.Name);
                    if (matchingField != null)
                    {
                        matchingField.Text = comboBox.SelectedItem?.ToString();
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
                    this.config.GeneralSettings.SavePath = fbd.SelectedPath;
                    Config.Update(this.config);
                    Console.WriteLine($"Files will be saved at: {fbd.SelectedPath}");
                }
            }
        }

        public void CalculateWindowSize()
        {
            if (this.config == null) { return; }
            float windowHeight = console.Height;
            windowHeight += this.MainMenuStrip.Height;
            // find max count of fields and forms
            int maxFields = 0;
            int maxForms = 0;
            foreach (var tab in config.Tabs)
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
}
