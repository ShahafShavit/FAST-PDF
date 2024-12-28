using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;
using System.Drawing.Text;
using System.Text;
using System.Text.RegularExpressions;
using static Org.BouncyCastle.Math.EC.ECCurve;

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
    public string? Text { get; set; }
    [JsonIgnore]
    public bool Checked { get; set; }
    [JsonIgnore]
    public ComboBoxItem? SelectedItem { get; set; }
    public string? ActionType { get; set; }
    public string? Name { get; set; }
    public string? Label { get; set; }
    public string? Placeholder { get; set; }
    public string? DefaultText { get; set; }
    public string? Description { get; set; }
    public List<ComboBoxItem>? Items { get; set; }
    public PDFSettings? PDFSettings { get; set; }
}
public class TabObject
{
    public string? TabName { get; set; }
    public List<FormObject>? Forms { get; set; }
}
public class FormObject
{
    public string? FormName { get; set; }
    public string? FileName { get; set; } // "FileName.pdf"
    public string? Path { get; set; } // "Path/To/FileName.pdf" // defines path to file within default input path defined in settings
    public string? Checksum { get; set; }
    public List<InputField>? Fields { get; set; }

    public void FillForm(string outputName, string outputPath, string inputPath, iText.Kernel.Colors.Color color)
    {

        string fileinputPath = System.IO.Path.Combine(AppContext.BaseDirectory, inputPath, this.Path ?? "");  // Path to your existing PDF
        string fullOutputPath = System.IO.Path.Combine(outputPath, outputName);    // Path for the modified PDF
        if (!File.Exists(fileinputPath))
        {
            Console.WriteLine($"Could not locate file {fileinputPath} | Please check further.");
            throw new FileNotFoundException(fileinputPath);
        }
        using (PdfReader reader = new PdfReader(fileinputPath))
        using (PdfWriter writer = new PdfWriter(fullOutputPath))
        using (PdfDocument pdf = new PdfDocument(reader, writer))
        {
            foreach (InputField inputField in this.Fields)
            {
                var font = Utility.LoadSystemFont(inputField.PDFSettings.Font);
                var formattedText = Utility.ReverseRtlString(inputField.Text);

                float fontSize = inputField.PDFSettings.Size;

                var locations = inputField.PDFSettings.Locations;

                if (inputField.PDFSettings.ResizeFunctionUse)
                {
                    fontSize = GetFontSize(formattedText.Length);
                }
                if (inputField.Type == "CheckBox" && !inputField.Checked && inputField.ActionType == "Check") continue;
                else if (inputField.Type == "ComboBox")
                {
                    formattedText = Utility.ReverseInput(inputField.SelectedItem.Text);
                    locations = inputField.SelectedItem.Locations;
                }

                foreach (Location c in locations)
                {
                    var page = pdf.GetPage(c.Page);
                    var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);

                    float x = c.X;
                    float y = c.Y;
                    if (c.Width.HasValue && c.Height.HasValue)
                    {
                        if (c.Type == InputField.ShapeType.Rectangle)
                        {
                            iText.Kernel.Geom.Rectangle rect = new iText.Kernel.Geom.Rectangle(((int)x), ((int)y), -c.Width.Value, c.Height.Value);
                            canvas.SetStrokeColor(color);
                            canvas.SetLineWidth(1);
                            canvas.Rectangle(rect);
                            canvas.Stroke();
                        }
                        else if (c.Type == InputField.ShapeType.Ellipse)
                        {
                            canvas.SetStrokeColor(color);
                            canvas.SetLineWidth(1);
                            canvas.Ellipse(c.X, c.Y, c.X - c.Width.Value, c.Y + c.Height.Value);
                            canvas.Stroke();
                        }
                    }
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

                foreach (Location c in inputField.PDFSettings.Locations)
                {
                    var page = pdf.GetPage(c.Page);

                    var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);

                    float x = c.X;
                    float y = c.Y;

                    var font = Utility.LoadSystemFont(inputField.PDFSettings.Font);
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
    public static int GetFontSize(int x)
    {
        if (x < 8) return 10;
        double realvalue = (1 / (0.05 * x + 0.1)) + 8;
        return (int)Math.Ceiling(realvalue);
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
public class PDFSettings
{
    public string? Font { get; set; }
    public int Size { get; set; }
    public bool ResizeFunctionUse { get; set; }
    public bool Required { get; set; }
    public bool RTL { get; set; }
    public List<Location>? Locations { get; set; }
}
public class Location
{
    public int Page { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public InputField.ShapeType? Type {  get; set; }
}

public class ComboBoxItem
{
    public string? Label { get; set; }
    public string? Text { get; set; }
    public List<Location>? Locations { get; set; }

    public override string ToString()
    {
        return Label ?? "null"; // Display Label in ComboBox dropdown
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