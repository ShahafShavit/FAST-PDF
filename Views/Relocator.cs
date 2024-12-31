#pragma warning disable CS8602
public partial class Relocator : UserControl
{

    public Relocator(Location location, string LocationName)
    {
        int page = location.Page; float x = location.X; float y = location.Y;
        InitializeComponent();
        RelocatorName.Text = LocationName;
        if (LocationName.Length >18)
            RelocatorName.Text = LocationName.Substring(0, 18);

        PAGE_TB.Text = page.ToString();
        X_TB.Text = x.ToString();
        Y_TB.Text = y.ToString();
        up_btn.Click += (s, e) =>
        {
            Y_TB.Text = CalcNew(Y_TB.Text, true); UpdateValue(location, "Y", Y_TB.Text);
        };
        down_btn.Click += (s, e) => { Y_TB.Text = CalcNew(Y_TB.Text, false); UpdateValue(location, "Y", Y_TB.Text); };
        right_btn.Click += (s, e) => { X_TB.Text = CalcNew(X_TB.Text, true); UpdateValue(location, "X", X_TB.Text); };
        left_btn.Click += (s, e) => { X_TB.Text = CalcNew(X_TB.Text, false); UpdateValue(location, "X", X_TB.Text); };
        PAGE_TB.Click += (s, e) => { UpdateValue(location, "PAGE", PAGE_TB.Text); };
        Y_TB.KeyPress += (s, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((s as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        };
        X_TB.KeyPress += (s, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((s as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        };
        X_TB.TextChanged += (s, e) => { UpdateValue(location, "X", X_TB.Text); };
        Y_TB.TextChanged += (s, e) => { UpdateValue(location, "Y", Y_TB.Text); };
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
    private void UpdateValue(Location location, string val, string value)
    {
        float newValue;
        if (string.IsNullOrEmpty(value)) { newValue = 0; }
        else { newValue = float.Parse(value); }
        if (val == "Y")
        {
            location.Y = newValue;
        }
        else if (val == "X")
        {
            location.X = newValue;
        }
        else if (val == "PAGE")
        {
            location.Page = ((int)newValue);
        }
    }

}

