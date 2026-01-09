using System.Text.Json;

public static class FileStorage
{
    public static List<T> LoadData<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<T>();
        }

        var jsonData = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
    }

    public static void SaveData<T>(string filePath, List<T> data)
    {
        var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        File.WriteAllText(filePath, jsonData);
    }
}