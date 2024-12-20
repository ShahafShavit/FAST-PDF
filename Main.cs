using Newtonsoft.Json;
using System.Text;
using System.Windows.Forms;
namespace Auto_UI_Test
{
    public partial class Main : System.Windows.Forms.Form
    {
        private TextBox console;
        private UIConfig config;
        public Main()
        {
            InitializeComponent();
            ReadConfig();
            GenerateUI();
            RedirectConsoleOutput();
            Console.WriteLine("Initialization of components has been completed.");
            this.Width = 1200;
            this.RightToLeft = RightToLeft.Yes;
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

                TableLayoutPanel tabLayoutPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    //FlowDirection = FlowDirection.LeftToRight, // Horizontal arrangement
                    ColumnCount = 4,
                    RowCount = 1,
                    Padding = new Padding(2),
                    Margin = new Padding(1),

                    //WrapContents = true // Ensures wrapping if it overflows horizontally
                };
                tabLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                tabLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                tabLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                tabLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                foreach (var group in tab.Forms)
                {
                    GroupBox groupBox = new GroupBox
                    {
                        Text = group.FormName,
                        Dock = DockStyle.Fill,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowOnly,
                        Size = new Size(300, 700),
                        Padding = new Padding(4),
                        Margin = new Padding(2),
                        Tag = group.Path, // Tag holds the path to the file
                    };

                    TableLayoutPanel layout = new TableLayoutPanel
                    {
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        ColumnCount = 2,
                        Dock = DockStyle.Fill,

                    };

                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
                    int row = 0;
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
                                Margin = new Padding(2)
                            };

                            Control control = ControlFactory.CreateControlFromJson(field);
                            control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                            control.Margin = new Padding(2);
                            control.Dock = DockStyle.Top;

                            layout.Controls.Add(label, 0, row); // Add label in the first column
                            layout.Controls.Add(control, 1, row); // Add control in the second column
                            row++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error creating control: {ex.Message}");
                        }
                    }
                    Label fileNameLabel = new Label
                    {
                        Text = "Filename:",
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
                        Tag = "FileName"
                    };

                    layout.Controls.Add(fileNameLabel, 0, row);
                    layout.Controls.Add(fileNameTextBox, 1, row);
                    row++;
                    var generateButton = new Button
                    {
                        Text = "Generate",
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
            mainLayout.Controls.Add(console, 0, 2);
            this.Controls.Add(mainLayout);
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

            Console.WriteLine($"New filename to save as: {newFilename}");


            FillDataStructure(layoutPanel.Controls, parentGroupBox.Text, clickedButton.Parent.Parent.Parent.Text);
        }
        private void RedirectConsoleOutput()
        {
            Console.SetOut(new TextBoxWriter(console));
        }
        private MenuStrip GenerateMenuStrip()
        {

            // Create a new MenuStrip
            MenuStrip menuStrip = new MenuStrip();
            //menuStrip.Dock = DockStyle.Top;

            // Create "File" menu
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem chooseSaveFolder = new ToolStripMenuItem("שמור בתיקייה");
            //ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Save");
            //ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Exit");

            chooseSaveFolder.Click += (s, e) => SaveFolderDialoge();
            // Add event handlers for menu items
            //openMenuItem.Click += (s, e) => OpenFile();
            //saveMenuItem.Click += (s, e) => SaveFile();
            //exitMenuItem.Click += (s, e) => ExitApplication();

            // Add sub-items to "File" menu
            //fileMenu.DropDownItems.Add(openMenuItem);
            //fileMenu.DropDownItems.Add(saveMenuItem);
            //fileMenu.DropDownItems.Add(new ToolStripSeparator()); // Adds a separator
            fileMenu.DropDownItems.Add(chooseSaveFolder);

            // Create "Edit" menu
            ToolStripMenuItem editMenu = new ToolStripMenuItem("Edit");
            ToolStripMenuItem cutMenuItem = new ToolStripMenuItem("Cut");
            ToolStripMenuItem copyMenuItem = new ToolStripMenuItem("Copy");
            ToolStripMenuItem pasteMenuItem = new ToolStripMenuItem("Paste");

            // Add event handlers for "Edit" menu items
            //cutMenuItem.Click += (s, e) => Cut();
            //copyMenuItem.Click += (s, e) => Copy();
            //pasteMenuItem.Click += (s, e) => Paste();

            // Add sub-items to "Edit" menu
            editMenu.DropDownItems.Add(cutMenuItem);
            editMenu.DropDownItems.Add(copyMenuItem);
            editMenu.DropDownItems.Add(pasteMenuItem);

            // Create "View" menu
            ToolStripMenuItem viewMenu = new ToolStripMenuItem("View");
            ToolStripMenuItem toggleDarkMode = new ToolStripMenuItem("Toggle Dark Mode");
            //toggleDarkMode.Click += (s, e) => ToggleDarkMode();
            viewMenu.DropDownItems.Add(toggleDarkMode);

            // Add top-level menus to the MenuStrip
            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(editMenu);
            menuStrip.Items.Add(viewMenu);

            // Add the MenuStrip to the form
            this.MainMenuStrip = menuStrip;
            return menuStrip;

        }
        private void FillDataStructure(TableLayoutControlCollection controls, string formName, string tabName)
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
                if (control is TextBox textBox)
                {
                    var matchingField = formObject.Fields?.Find(field => field.Name == control.Name);
                    if (matchingField != null)
                    {
                        matchingField.Text = textBox.Text;
                        Console.WriteLine($"Found and filled the {matchingField} object with {control.Text}");
                    }
                }
                else if (control is CheckBox checkBox)
                {
                    var matchingField = formObject.Fields?.Find(field => field.Name == control.Name);
                    if (matchingField != null)
                    {
                        matchingField.Text = checkBox.Checked.ToString();
                        Console.WriteLine($"Found and filled the {matchingField} object with {checkBox.Checked.ToString()}");
                    }
                }
                else if (control is ComboBox comboBox)
                {
                    var matchingField = formObject.Fields?.Find(field => field.Name == control.Name);
                    if (matchingField != null)
                    {
                        matchingField.Text = comboBox.SelectedItem?.ToString();
                        Console.WriteLine($"Found and filled the {matchingField} object with {comboBox.SelectedItem.ToString()}");
                    }
                }
            }
            // formObject.GeneratePDF();
        }
        private void SaveFolderDialoge()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                DialogResult dr = fbd.ShowDialog();
                if (dr == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    this.config.GeneralSettings.SavePath = fbd.SelectedPath;
                    UpdateConfig();
                    Console.WriteLine($"Files will be saved at: {fbd.SelectedPath}");
                }
            }
        }
        private UIConfig LoadConfiguration()
        {
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "config.json");
            return JsonConvert.DeserializeObject<UIConfig>(File.ReadAllText(jsonPath));
        }
        public class TextBoxWriter : TextWriter
        {
            private readonly TextBox _textBox;

            public TextBoxWriter(TextBox textBox)
            {
                _textBox = textBox;
            }

            public override Encoding Encoding => Encoding.UTF8;

            public override void Write(char value)
            {
                if (_textBox.InvokeRequired)
                {
                    _textBox.BeginInvoke(new Action(() => _textBox.AppendText(value.ToString())));
                }
                else
                {
                    _textBox.AppendText(value.ToString());
                }
            }

            public override void Write(string value)
            {
                value = value.Replace("\n", "\r\n");
                if (_textBox.InvokeRequired)
                {
                    _textBox.BeginInvoke(new Action(() => _textBox.AppendText(value)));
                }
                else
                {
                    _textBox.AppendText(value);
                }
            }
        }
        public void UpdateConfig()
        {
            string json = JsonConvert.SerializeObject(config,Formatting.Indented);
            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "config.json"), json);
        }
        public void ReadConfig()
        {
            this.config = LoadConfiguration();
        }
    }
}
