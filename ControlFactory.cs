using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

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
