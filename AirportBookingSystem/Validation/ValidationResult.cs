public class ValidationError
{
    public string PropertyName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<ValidationError> Errors { get; } = new();
}