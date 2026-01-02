using System.Reflection;

public static class ObjectValidator
{
    public static ValidationResult Validate(object obj)
    {
        var result = new ValidationResult();
        var properties = obj.GetType().GetProperties();

        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes<ValidationAttribute>();
            
            foreach (var attribute in attributes)
            {
                if (!attribute.IsValid(property.GetValue(obj)))
                {
                    result.Errors.Add(new ValidationError
                    {
                        PropertyName = property.Name,
                        Message = attribute.Description
                    });
                }
            }
        }

        return result;
    }
}