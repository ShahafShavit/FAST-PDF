using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;


public static class Config
{

    public static UIConfig Pull()
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "config.json");
        return JsonConvert.DeserializeObject<UIConfig>(File.ReadAllText(jsonPath));
    }
    public static void Update(UIConfig config)
    {
        string json = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "config.json"), json);
    }


}
public static class Utility
{
    public static string ReverseRtlString(string input)
    {
        // Reverse the entire string
        char[] reversed = input.ToCharArray();
        Array.Reverse(reversed);
        string reversedString = new string(reversed);

        // Use a regex to reverse numbers back to their original order
        string formatted = Regex.Replace(reversedString, @"\d+", match =>
        {
            char[] numArray = match.Value.ToCharArray();
            Array.Reverse(numArray);
            return new string(numArray);
        });

        return formatted;
    }
    public static bool IsDate(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        string pattern = @"^(0[1-9]|[12][0-9]|3[01])[\/.\-](0[1-9]|1[0-2])[\/.\-](\d{2})$";
        return Regex.IsMatch(input, pattern);
    }
    public static bool IsEnglish(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        return Regex.IsMatch(input, "^[a-zA-Z0-9]*$");
    }
    public static string ReverseInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        StringBuilder sb = new StringBuilder();
        string[] arr = input.Split(" ");
        bool hebrew = false;
        foreach (string item in arr)
        {

            if (!string.IsNullOrEmpty(item))
            {
                if (IsDate(item) || IsEnglish(item))
                {
                    sb.Append(item);
                }
                else
                {
                    sb.Append(ReverseRtlString(item));
                    hebrew = true;
                }
                sb.Append(' ');
            }
        }
        if (hebrew)
        {
            string[] newArr = sb.ToString().Split(" ");
            Array.Reverse(newArr);
            string output = string.Join(" ", newArr);
            return output.Trim();
        }
        else
        {
            return sb.ToString().Trim();
        }
    }
    public static PdfFont LoadSystemFont(string fontName)
    {
        InstalledFontCollection fonts = new InstalledFontCollection();
        foreach (var fontFamily in fonts.Families)
        {
            if (fontFamily.Name.Equals(fontName, StringComparison.InvariantCultureIgnoreCase))
            {
                string fontPath = GetFontFilePath(fontFamily.Name);

                if (!string.IsNullOrEmpty(fontPath))
                {
                    FontProgram fontProgram = FontProgramFactory.CreateFont(fontPath);
                    return PdfFontFactory.CreateFont(fontProgram, PdfEncodings.IDENTITY_H);
                }
            }
        }

        throw new IOException($"Font \"{fontName}\" not found.");
    }
    public static string GetFontFilePath(string fontName)
    {
        string fontsFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
        string[] fontFiles = Directory.GetFiles(fontsFolder, "*.ttf");

        foreach (string fontFile in fontFiles)
        {
            if (System.IO.Path.GetFileNameWithoutExtension(fontFile).Equals(fontName, StringComparison.InvariantCultureIgnoreCase))
            {
                return fontFile;
            }
        }

        throw new FileNotFoundException("Unable to locate font file path");
    }
}

public class InputField
{
    public string Type { get; set; }
    [JsonIgnore]
    public string Text { get; set; }
    [JsonIgnore]
    public bool Checked { get; set; }
    [JsonIgnore]
    public ComboBoxItem SelectedItem { get; set; }
    public string ActionType { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public string Placeholder { get; set; }
    public string DefaultText { get; set; }
    public string Description { get; set; }
    public List<ComboBoxItem> Items { get; set; }
    public PDFSettings PDFSettings { get; set; }
}
public class TabObject
{
    public string TabName { get; set; }
    public List<FormObject> Forms { get; set; }
}
public class FormObject
{
    public string FormName { get; set; }
    public string FileName { get; set; } // "FileName.pdf"
    public string Path { get; set; } // "Path/To/FileName.pdf" // defines path to file within default input path defined in settings
    public string Checksum { get; set; }
    public List<InputField> Fields { get; set; }

    public void FillForm(string outputName, string outputPath, string inputPath, iText.Kernel.Colors.Color color)
    {

        string fileinputPath = System.IO.Path.Combine(AppContext.BaseDirectory, inputPath, this.Path);  // Path to your existing PDF
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

                if (inputField.Type == "CheckBox" && !inputField.Checked && inputField.ActionType == "Check") continue;
                else if (inputField.Type == "ComboBox")
                {
                    //inputField.SelectedItem

                    foreach (Location c in inputField.SelectedItem.Locations)
                    {
                        var page = pdf.GetPage(c.Page);

                        var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);

                        float x = c.X;
                        float y = c.Y;

                        var font = Utility.LoadSystemFont(inputField.PDFSettings.Font);
                        var formattedText = "";
                        formattedText = Utility.ReverseInput(inputField.SelectedItem.Text);

                        Rectangle textbox = new Rectangle(((int)x), ((int)y), 100, 200);
                        float fontSize = inputField.PDFSettings.Size;
                        if (inputField.PDFSettings.SizeFunctionUse) {
                            fontSize = GetFontSize(formattedText.Length);
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
                else
                {
                    foreach (Location c in inputField.PDFSettings.Location)
                    {
                        var page = pdf.GetPage(c.Page);

                        var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);

                        float x = c.X;
                        float y = c.Y;

                        var font = Utility.LoadSystemFont(inputField.PDFSettings.Font);
                        var formattedText = "";
                        formattedText = Utility.ReverseInput(inputField.Text);

                        Rectangle textbox = new Rectangle(((int)x), ((int)y), 100, 200);
                        float fontSize = inputField.PDFSettings.Size;
                        if (inputField.PDFSettings.SizeFunctionUse)
                        {
                            fontSize = GetFontSize(formattedText.Length);
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

                foreach (Location c in inputField.PDFSettings.Location)
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
public class UIConfig
{
    public GeneralSettings GeneralSettings { get; set; }
    public List<TabObject> Tabs { get; set; }

}

public class GeneralSettings
{
    public string SavePath { get; set; }
    public string InputPath { get; set; }
    public bool Debug { get; set; }
    public bool LaunchFileAtGeneration { get; set; }
}
public class PDFSettings
{
    public string Font { get; set; }
    public int Size { get; set; }
    public bool SizeFunctionUse { get; set; }
    public bool Required { get; set; }
    public bool RTL { get; set; }
    public List<Location> Location { get; set; }
}
public class Location
{
    public int Page { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
}
public class ComboBoxItem
{
    public string Label { get; set; }
    public string Text { get; set; }
    public List<Location> Locations { get; set; }

    public override string ToString()
    {
        return Label; // Display Label in ComboBox dropdown
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