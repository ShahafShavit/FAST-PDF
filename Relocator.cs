using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public partial class Relocator : UserControl
{

    public Relocator(Location location)
    {
        int page = location.Page; float x = location.X; float y = location.Y;
        InitializeComponent();
        PAGE_TB.Text = page.ToString();
        X_TB.Text = x.ToString();
        Y_TB.Text = y.ToString();
        up_btn.Click += (s, e) => {
            Y_TB.Text = CalcNew(Y_TB.Text, true); UpdateValue(location, "Y", float.Parse(Y_TB.Text)); };
        down_btn.Click += (s, e) => { Y_TB.Text = CalcNew(Y_TB.Text, false); UpdateValue(location, "Y", float.Parse(Y_TB.Text)); };
        right_btn.Click += (s, e) => { X_TB.Text = CalcNew(X_TB.Text, true); UpdateValue(location, "X", float.Parse(X_TB.Text)); };
        left_btn.Click += (s, e) => { X_TB.Text = CalcNew(X_TB.Text, false); UpdateValue(location, "X", float.Parse(X_TB.Text)); };
        PAGE_TB.Click += (s, e) => { UpdateValue(location, "PAGE", float.Parse(PAGE_TB.Text)); };
    }
    private string CalcNew(string num, bool increase)
    {
        float newnum = float.Parse(num);
        if (increase)
        {
            return (newnum += 1).ToString();
        }
        return (newnum -= 1).ToString();
    }
    private void UpdateValue(Location location,string val, float value)
    {
        if (val == "Y")
        {
            location.Y = value;
        }
        else if (val == "X") {
            location.X = value;
        }
        else if (val == "PAGE")
        {
            location.Page = ((int)value);
        }
    }

}

