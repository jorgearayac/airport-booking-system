public static class ValidationMetadataReader
{
    public static Dictionary<string, List<string>> GetValidationMetadata<T>()
    {
        var metadata = new Dictionary<string, List<string>>();
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            var attributes = prop.GetCustomAttributes(true);
            var attrNames = new List<string>();

            foreach (var attr in attributes)
            {
                attrNames.Add(attr.GetType().Name);
            }

            metadata[prop.Name] = attrNames;
        }

        return metadata;
    }

    public static void PrintValidationMetadata<T>()
    {
        var metadata = GetValidationMetadata<T>();
        foreach (var entry in metadata)
        {
            Console.WriteLine($"Property: {entry.Key}");
            Console.WriteLine("Attributes: " + string.Join(", ", entry.Value));
            Console.WriteLine();
        }
    }
}