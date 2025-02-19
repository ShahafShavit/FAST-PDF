using BidiSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class BidiString
{
    public string Original { get; }
    public string Visual { get; }

    public BidiString(string text)
    {
        Original = text;
        Visual = Bidi.LogicalToVisual(text);
    }

    public override string ToString() => Visual;
}

