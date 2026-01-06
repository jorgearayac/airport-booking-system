public static class ValidationMetadataReader
{
    public static List<ValidationDetail> GetValidationMetadata<T>()
    {
        var result = new List<ValidationDetail>();
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            var field = new ValidationDetail()
            {
                PropertyName = prop.Name,
                Type = GetFriendlyTypeName(prop.PropertyType)
            };
            
            var attributes = prop.GetCustomAttributes(true);
            var attrNames = new List<string>();

            foreach (var attr in attributes)
            {
                field.Constraints.Add(GetConstraintDescription(attr));
            }

            result.Add(field);
        }

        return result;
    }

    private static string GetConstraintDescription(object attribute)
    {
        return attribute switch
        {
            RequiredAttribute => "Required",
            FutureDateAttribute => "Must be in the future",
            PositiveNumberAttribute => "Must be a positive number",
            _ => attribute.GetType().Name
        };
    }

    private static string GetFriendlyTypeName(Type type)
    {
        if (type == typeof(string)) return "Free Text";
        if (type == typeof(DateTime)) return "Date Time";
        if (type == typeof(decimal)) return "Decimal Number";

        return type.Name;
    }
}