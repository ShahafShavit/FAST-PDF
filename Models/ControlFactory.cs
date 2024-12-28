using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection;

#pragma warning disable CS8600 //Converting null literal or possible null value to non-nullable type.	
#pragma warning disable CS8602
#pragma warning disable CS8603

public static class ControlFactory
{
    public static Control CreateControlFromJson(dynamic field)
    {
        // Map JSON `Type` to the corresponding WinForms control type
        string controlType = field.Type;
        Type type = Type.GetType($"System.Windows.Forms.{controlType}, System.Windows.Forms");

        if (type == null)
        {
            throw new Exception($"Control type '{controlType}' not recognized.");
        }

        // Create an instance of the control
        Control control = (Control)Activator.CreateInstance(type);

        // Dynamically set properties from the JSON
        foreach (var property in field.GetType().GetProperties())
        {
            string propertyName = property.Name;
            var propertyValue = property.GetValue(field);

            // Special case: ComboBox items
            if (type == typeof(ComboBox) && propertyName == "Items")
            {
                ComboBox comboBox = (ComboBox)control;
                foreach (var item in propertyValue)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem
                    {
                        Label = item.Label,
                        Text = item.Text,
                        Locations = item.Locations
                    };
                    comboBox.Items.Add(comboBoxItem);
                }

                comboBox.DataBindings.Add("SelectedItem", field, nameof(field.SelectedItem), false, DataSourceUpdateMode.OnPropertyChanged);
                continue;
            }
            if (propertyName == "Font" || propertyName == "Size") continue;
            // Check if the property exists in the control type
            PropertyInfo controlProperty = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (controlProperty != null && controlProperty.CanWrite)
            {
                // Handle special cases (like converting colors)
                if (controlProperty.PropertyType == typeof(Color))
                {
                    controlProperty.SetValue(control, ColorTranslator.FromHtml(propertyValue.ToString()));
                }
                else if (controlProperty.PropertyType == typeof(ContentAlignment))
                {
                    controlProperty.SetValue(control, Enum.Parse(typeof(ContentAlignment), propertyValue.ToString()));
                }
                else
                {
                    controlProperty.SetValue(control, Convert.ChangeType(propertyValue, controlProperty.PropertyType));
                }
            }
        }
        if (control is TextBox textBox)
        {
            //textBox.DataBindings.Add("Text", field, nameof(field.Text), false, DataSourceUpdateMode.OnPropertyChanged);
        }
        return control;
    }
}

