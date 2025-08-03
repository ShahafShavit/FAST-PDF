using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;

#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8600

public class InputField
{
    public enum ShapeType
    {
        Ellipse,
        Rectangle
    }
    public string? Type { get; set; }
    [JsonIgnore]
    public string Text { get; set; }
    [JsonIgnore]
    public bool Checked { get; set; }
    [JsonIgnore]
    public ComboBoxItem? SelectedItem { get; set; }
    public string? ActionType { get; set; }
    public string? Bank { get; set; }
    public string? Name { get; set; }
    public string? Label { get; set; }
    public string? DebugPlaceholder { get; set; }
    public string? DefaultText { get; set; }
    public string? Description { get; set; }
    public Person.DataType? DataType { get; set; }
    public string? Font { get; set; }
    public int Size { get; set; }
    public bool ResizeFunctionUse { get; set; }
    public List<Location>? Locations { get; set; }
    public List<ComboBoxItem>? Items { get; set; }
    public List<InputField>? SubFields { get; set; }


}
public class Personnel
{
    public BindingList<Person> PersonList { get; set; }
}
public class ClientsList
{
    public BindingList<Client> Clients { get; set; }
}
public class Client
{
    public string? Name { get; set; }
    public string? ID { get; set; }
    public string? Phone { get; set; }
    public string? HetPei { get; set; }
    public string? EmailAddress { get; set; }
    public override string ToString()
    {
        return Name ?? "Null";
    }
}

public class Person
{
    public enum DataType
    {
        Name,          // 0
        ID,            // 1
        Phone,         // 2
        LicenseType,   // 3
        LicenseNumber, // 4
        Address,       // 5
        EmailAddress,  // 6
        HetPei         // 7
    }
    public string? Name { get; set; }
    public string? ID { get; set; }
    public string? Phone { get; set; }
    public string? LicenseType { get; set; }
    public string? LicenseNumber { get; set; }
    public override string ToString()
    {
        return Name ?? "Null";
    }
}
public class TabObject
{
    public string? TabName { get; set; }
    public int Order { get; set; }
    public List<FormObject>? Forms { get; set; }
}
public class FormObject
{
    public int Order { get; set; }
    public string? FormName { get; set; }
    public string? FileName { get; set; } // "FileName.pdf"
    public string? Path { get; set; } // "Path/To/FileName.pdf" // defines path to file within default input path defined in settings
    public string? Checksum { get; set; }
    public List<InputField>? Fields { get; set; }

    public void FillForm(string outputName, string outputPath, string inputPath, iText.Kernel.Colors.Color color)
    {
        string fileInputPath = System.IO.Path.Combine(AppContext.BaseDirectory, inputPath, this.Path ?? "");
        string fullOutputPath = System.IO.Path.Combine(outputPath, outputName);

        if (!File.Exists(fileInputPath))
        {
            Console.WriteLine($"Could not locate file {fileInputPath} | Please check further.");
            throw new FileNotFoundException(fileInputPath);
        }

        using var reader = new PdfReader(fileInputPath);
        using var writer = new PdfWriter(fullOutputPath);
        using var pdf = new PdfDocument(reader, writer);

        foreach (InputField inputField in this.Fields)
        {
            ProcessFieldRecursive(inputField, pdf, color);
        }
    }

    private void ProcessFieldRecursive(InputField inputField, PdfDocument pdf, iText.Kernel.Colors.Color color)
    {
        var font = Utility.LoadSystemFont(inputField.Font);
        //string formattedText = Utility.ReverseInput(inputField.Text);
        string formattedText = new BidiString(inputField.Text).Visual;
        float fontSize = inputField.ResizeFunctionUse ? GetFontSize(formattedText.Length) : inputField.Size;
        var locations = inputField.Locations;
        if (inputField.Type == "CheckBox" && !inputField.Checked && inputField.ActionType == "Check")
        {
            return; // Skip unchecked checkboxes
        }
        if (inputField.Type == "ComboBox" && inputField.ActionType == "Selector")
        {
            formattedText = Utility.ReverseInput(inputField.SelectedItem.Text);
        }
        // Process current field

        locations = inputField.Type == "ComboBox" && inputField.ActionType == "Selector"
            ? inputField.SelectedItem.Locations
            : inputField.Locations;


        if (inputField.Type == "CheckBox" && inputField.ActionType == "Check")
        {
            formattedText = "V";
        }
        if (locations != null)
        {
            DrawContent(pdf, formattedText, locations, fontSize, font, color);
        }
        // Recursively process subfields
        if (inputField.SubFields != null && inputField.SubFields.Any())
        {
            foreach (var subField in inputField.SubFields)
            {
                ProcessFieldRecursive(subField, pdf, color);
            }
        }
    }

    private void DrawContent(PdfDocument pdf, string text, IEnumerable<Location> locations, float fontSize, PdfFont font, iText.Kernel.Colors.Color color)
    {
        foreach (var loc in locations)
        {
            PdfPage page;
            try
            {
                page = pdf.GetPage(loc.Page);
            }
            catch { Console.WriteLine($"Invalid Page selected for InputField text {text}"); return; }
            var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);

            if (loc.Width.HasValue && loc.Height.HasValue)
            {
                DrawShape(canvas, loc, color);
                return;
            }

            var paragraph = new Paragraph(text)
                .SetFont(font)
                .SetFontSize(fontSize)
                .SetFontColor(color)
                .SetBaseDirection(BaseDirection.RIGHT_TO_LEFT);

            var document = new iText.Layout.Document(pdf);
            document.ShowTextAligned(paragraph, loc.X, loc.Y, loc.Page, TextAlignment.RIGHT, iText.Layout.Properties.VerticalAlignment.BOTTOM, 0);
        }
    }

    private void DrawShape(iText.Kernel.Pdf.Canvas.PdfCanvas canvas, Location loc, iText.Kernel.Colors.Color color)
    {
        canvas.SetStrokeColor(color).SetLineWidth(1);

        if (loc.Type == InputField.ShapeType.Rectangle)
        {
            var rect = new iText.Kernel.Geom.Rectangle(loc.X, loc.Y, -loc.Width.Value, loc.Height.Value);
            canvas.Rectangle(rect).Stroke();
        }
        else if (loc.Type == InputField.ShapeType.Ellipse)
        {
            canvas.Ellipse(loc.X, loc.Y, loc.X - loc.Width.Value, loc.Y + loc.Height.Value).Stroke();
        }
    }



    public void FillSpecialForm(string outputFilename, string outputDirectory, string inputFilename, string defaultInputPath, iText.Kernel.Colors.Color color)
    {
        string fileDirectory = System.IO.Path.GetDirectoryName(this.Path);
        string fileInputPath = System.IO.Path.Combine(defaultInputPath, fileDirectory, inputFilename);  // Path to existing PDF
        string fullOutputPath = System.IO.Path.Combine(outputDirectory, outputFilename);    // Path for the modified PDF

        FileInfo fileInfo = new FileInfo(fileInputPath);

        using (PdfReader reader = new PdfReader(fileInfo))
        using (PdfWriter writer = new PdfWriter(fullOutputPath))
        using (PdfDocument pdf = new PdfDocument(reader, writer))
        {

            foreach (InputField inputField in this.Fields)
            {
                if (inputField.Type == "CheckBox" && !inputField.Checked && inputField.ActionType == "Check") continue;

                foreach (Location c in inputField.Locations)
                {
                    var page = pdf.GetPage(c.Page);

                    var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);

                    float x = c.X;
                    float y = c.Y;

                    var font = Utility.LoadSystemFont(inputField.Font);
                    var formattedText = Utility.ReverseInput(inputField.Text);
                    Rectangle textbox = new Rectangle(((int)x), ((int)y), 100, 200);
                    //formattedText = formattedText.Replace(" ", "\n");
                    float fontSize = GetFontSize(formattedText.Length);

                    Paragraph paragraph = new Paragraph(formattedText)
                        .SetFont(font)
                        .SetFontSize(fontSize)
                        .SetFontColor(color)
                        .SetBaseDirection(BaseDirection.RIGHT_TO_LEFT);

                    var document = new iText.Layout.Document(pdf);
                    document.ShowTextAligned(paragraph, x, y, c.Page, TextAlignment.RIGHT, iText.Layout.Properties.VerticalAlignment.BOTTOM, 0);
                }

            }

        }
    }
    public static int GetFontSize(int inputLength)
    {
        if (inputLength < 8) return 11;
        double realvalue = -0.12 * inputLength + 12.5;
        return (int)Math.Floor(realvalue);
    }
}
public class Models
{
    public List<TabObject>? Tabs { get; set; }
}

public class GlobalSettings
{
    public string? SavePath { get; set; }
    public string? InputPath { get; set; }
    public bool Debug { get; set; }
    public bool LaunchFileAtGeneration { get; set; }
}
public class Location
{
    public int Page { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public InputField.ShapeType? Type { get; set; }
}

public class ComboBoxItem
{
    public string? Label { get; set; }
    public string? Text { get; set; }
    public List<Location>? Locations { get; set; }

    public override string ToString()
    {
        return Label ?? "null"; // Display Label in ComboBox dropdown // commit
    }
}
public class TextBoxWriter : TextWriter
{
    private readonly System.Windows.Forms.TextBox _textBox;

    public TextBoxWriter(System.Windows.Forms.TextBox textBox)
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

    public override void Write(string? value)
    {
        value = value?.Replace("\n", "\r\n");
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