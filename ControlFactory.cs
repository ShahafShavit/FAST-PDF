using System.Reflection;

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

                // Set event to handle selection and custom rendering if needed
                comboBox.SelectedIndexChanged += (sender, args) =>
                {
                    var selectedItem = comboBox.SelectedItem as ComboBoxItem;
                    if (selectedItem != null)
                    {
                        Console.WriteLine($"Selected: {selectedItem.Label}, Location X: {selectedItem.Locations[0].X} Y: {selectedItem.Locations[0].Y} PAGE: {selectedItem.Locations[0].Page}");
                    }
                };

                continue;
            }

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

        return control;
    }
}

