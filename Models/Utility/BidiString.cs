using BidiSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


internal class BidiString
{
    // Bidi class B. Bidi.LogicalToVisual empties its direction stack at these characters.
    private static readonly char[] ParagraphSeparators = { '\n', '\r', '\u001C', '\u001D', '\u001E', '\u0085', '\u2029' };
    private static readonly Regex ParagraphSeparatorRuns = new Regex("[\n\r\u001C\u001D\u001E\u0085\u2029]+", RegexOptions.Compiled);

    public string Original { get; }
    public string Visual { get; }

    public BidiString(string text)
    {
        Original = text;
        Visual = Bidi.LogicalToVisual(RemoveParagraphSeparators(text));
    }

    private static string RemoveParagraphSeparators(string text)
    {
        if (string.IsNullOrEmpty(text) || text.IndexOfAny(ParagraphSeparators) < 0)
            return text;

        return ParagraphSeparatorRuns.Replace(text.Trim(ParagraphSeparators), " ");
    }

    public override string ToString() => Visual;
}
