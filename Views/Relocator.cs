#pragma warning disable CS8602
public partial class Relocator : UserControl
{

    public Relocator(Location location, string LocationName)
    {
        int page = location.Page; float x = location.X; float y = location.Y;

        InitializeComponent();
        RelocatorName.Text = LocationName;
        if (LocationName.Length > 18)
            RelocatorName.Text = LocationName.Substring(0, 18);

        //if (location.Width == null || location.Height == null || location.Type == null)
        //{
        if (location.Width == null) WIDTH_TB.Text = string.Empty;
        else
        {
            WIDTH_TB.Text = location.Width.ToString();
        }
        if (location.Height == null) HEIGHT_TB.Text = string.Empty;
        else
        {
            HEIGHT_TB.Text = location.Height.ToString();
        }
        if (location.Type == null) ShapeSelector.Text = "null";
        else
        {
            ShapeSelector.Text = location.Type.ToString();
        }

        PAGE_TB.Text = page.ToString();
        X_TB.Text = x.ToString();
        Y_TB.Text = y.ToString();
        up_btn.Click += (s, e) => { Y_TB.Text = CalcNew(Y_TB.Text, true); UpdateValue(location, "Y", Y_TB.Text); };
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
        WIDTH_TB.KeyPress += (s, e) =>
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
        HEIGHT_TB.KeyPress += (s, e) =>
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
        WIDTH_TB.TextChanged += (s, e) => { UpdateValue(location, "WIDTH", WIDTH_TB.Text); };
        HEIGHT_TB.TextChanged += (s, e) => { UpdateValue(location, "HEIGHT", HEIGHT_TB.Text); };
        ShapeSelector.SelectedIndexChanged += (s, e) => { UpdateValue(location, "SHAPE", ShapeSelector.Text); };
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
    private void UpdateValue(Location location, string property, string value)
    {
        float newValue;
        if (string.IsNullOrEmpty(value)) { newValue = 0; }
        else if (float.TryParse(value, out newValue))
        {
            if (property == "Y")
            {
                location.Y = newValue;
            }
            else if (property == "X")
            {
                location.X = newValue;
            }
            else if (property == "PAGE")
            {
                location.Page = ((int)newValue);
            }
            else if (property == "WIDTH")
            {
                if (newValue == 0)
                {
                    location.Width = null;
                }
                else
                {
                    location.Width = ((int)newValue);
                }
            }
            else if (property == "HEIGHT")
            {
                if (newValue == 0)
                {
                    location.Height = null;
                }
                else
                {
                    location.Height = ((int)newValue);
                }
            }
        }
        if (property == "SHAPE")
        {
            if (value == "null")
            {
                location.Type = null;
            }
            else
            {
                location.Type = (InputField.ShapeType)Enum.Parse(typeof(InputField.ShapeType), value);
            }
        }
    }

    private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
    {

    }
}

