using System.Reflection;
using Newtonsoft.Json;

namespace WoogaTakeHome.Resources;

public class TestData
{
    public static List<string> LoadResultCompareTerms(string languageIdentifier)
    {
        // Dynamically getting the path of the executing assembly and navigating to the file location
        string basePath = Directory.GetCurrentDirectory(); // Gets the current working directory of the application
        string jsonFilePath = Path.Combine(basePath, "Resources", "resultCompareTerms.json");
        // Read the content of the JSON file
        try
        {
            var jsonData = File.ReadAllText(jsonFilePath);
            // Deserialize into a dictionary of dictionaries
            var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonData);
    
            // Check if the specified language code exists in the dictionary
            if (data != null && data.ContainsKey(languageIdentifier) && data[languageIdentifier].ContainsKey("locations"))
            {
                return data[languageIdentifier]["locations"];
            }
            throw new KeyNotFoundException($"The specified language code '{languageIdentifier}' " +
                                           $"does not exist or it does not contain 'locations'");
        }
        catch (IOException ex)
        {
            Console.WriteLine("An error occurred while reading the file: " + ex.Message);
            return new List<string>();
        }
       
    }
}