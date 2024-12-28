using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8602 // Possible null reference return.
public static class Config
{
    private static string modelsFile = "Data\\models.json";
    private static string settingsFile = "Data\\settings.json";
    public static GlobalSettings PullSettings()
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, settingsFile);

        return JsonConvert.DeserializeObject<GlobalSettings>(File.ReadAllText(jsonPath));
    }
    public static void UpdateSettings(GlobalSettings settings)
    {
        string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        File.WriteAllText(Path.Combine(AppContext.BaseDirectory, settingsFile), json);
    }
    public static void UpdateModels(Models models)
    {
        string json = JsonConvert.SerializeObject(models, Formatting.Indented);
        File.WriteAllText(Path.Combine(AppContext.BaseDirectory, modelsFile), json);
    }
    public static Models PullModels()
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, modelsFile);
        return JsonConvert.DeserializeObject<Models>(File.ReadAllText(jsonPath));
    }
    public static void DeveloperSwap()
    {
        string debugFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, modelsFile);
        string solutionFilePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, modelsFile);

        File.Copy(debugFilePath, solutionFilePath, true);

        debugFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, settingsFile);
        solutionFilePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, settingsFile);

        File.Copy(debugFilePath, solutionFilePath, true);
    }

}
