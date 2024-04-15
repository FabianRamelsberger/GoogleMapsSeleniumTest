using System.Reflection;
using Newtonsoft.Json;

namespace WoogaTakeHome.Resources;

public class TestData
{
    public static List<string> LoadSearchTerms()
    {
        // Dynamically getting the path of the executing assembly and navigating to the file location
        string basePath = Directory.GetCurrentDirectory(); // Gets the current working directory of the application
        string jsonFilePath = Path.Combine(basePath, "Resources", "searchTerms.json");
        // Read the content of the JSON file
        try
        {
            var jsonData = File.ReadAllText(jsonFilePath);
            var data = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonData);
            return data["locations"];
        }
        catch (IOException ex)
        {
            Console.WriteLine("An error occurred while reading the file: " + ex.Message);
            return new List<string>();
        }
       
    }
}