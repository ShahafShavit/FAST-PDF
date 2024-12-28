using iText.IO.Font;
using iText.Kernel.Font;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


public static class Utility
{
    public static string ReverseHebrewText(string input)
    {
        // Regex pattern to match Hebrew words, with optional surrounding punctuation
        var hebrewWordPattern = new Regex(@"[\u0590-\u05FF]+");

        // Function to reverse the characters in a Hebrew word
        string ReverseWord(string word)
        {
            char[] charArray = word.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        // Split the input into "tokens" (words and separators)
        var tokens = Regex.Split(input, @"(\s+)"); // Keeps spaces as tokens

        // Process tokens: reverse Hebrew words and their order
        for (int i = 0; i < tokens.Length; i++)
        {
            if (hebrewWordPattern.IsMatch(tokens[i]))
            {
                tokens[i] = ReverseWord(tokens[i]); // Reverse individual Hebrew words
            }
        }

        // Reverse the order of Hebrew word sequences in the tokens
        var hebrewReversedTokens = new Regex(@"\s*([\u0590-\u05FF\s]+)\s*")
            .Replace(string.Join("", tokens), match =>
            {
                var words = match.Groups[1].Value.Split(' ');
                Array.Reverse(words);
                return string.Join(" ", words);
            });

        return hebrewReversedTokens;
    }
    public static string ReverseRtlString(string input)
    {
        char[] reversed = input.ToCharArray();
        Array.Reverse(reversed);
        string reversedString = new string(reversed);

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


