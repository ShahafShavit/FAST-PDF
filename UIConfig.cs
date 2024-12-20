using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;

public class InputField
{
    public string Type { get; set; }
    [JsonIgnore]
    public string Text { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
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
    public string FileName { get; set; }
    public string Path { get; set; }
    public string Checksum { get; set; }
    public List<InputField> Fields { get; set; }
    /*
    public void FillForm(string outputName, string outputPath)
    {

        string inputPath = this.Path;  // Path to your existing PDF
        string fullOutputPath = System.IO.Path.Combine(outputPath, outputName);    // Path for the modified PDF


        using (PdfReader reader = new PdfReader(inputPath))
        using (PdfWriter writer = new PdfWriter(fullOutputPath))
        using (PdfDocument pdf = new PdfDocument(reader, writer))
        {
            foreach (KeyValuePair<string, TextField> pair in this.TextFields)
            {
                string fieldName = pair.Key;
                TextField field = pair.Value;
                foreach (Coordinate c in field.Coordinates)
                {
                    var page = pdf.GetPage(c.Page);

                    var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);


                    float x = c.X;
                    float y = c.Y;

                    var font = Utility.LoadSystemFont(field.Font);
                    var formattedText = "";
                    if (Utility.IsDate(field.Text))
                    {
                        formattedText = field.Text;
                    }
                    else
                    {
                        formattedText = Utility.ReverseRtlString(field.Text);
                    }

                    Paragraph paragraph = new Paragraph(formattedText)
                        .SetFont(font)
                        .SetFontSize(field.Size)
                        .SetBaseDirection(BaseDirection.RIGHT_TO_LEFT);


                    var document = new iText.Layout.Document(pdf);
                    document.ShowTextAligned(paragraph, x, y, c.Page, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
                }

            }

        }
    }
    */
}
public class UIConfig
{
    public GeneralSettings GeneralSettings { get; set; }
    public List<TabObject> Tabs { get; set; }

}


public class GeneralSettings
{
    public string SavePath { get; set; }
}

public class PDFSettings
{
    public string Font { get; set; }
    public int Size { get; set; }
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
