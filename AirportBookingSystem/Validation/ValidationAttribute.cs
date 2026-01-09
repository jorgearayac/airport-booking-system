using System;

[AttributeUsage(AttributeTargets.Property)]
public abstract class ValidationAttribute : Attribute
{
    public abstract bool IsValid(object? value);
    public abstract string Description { get; }
}

public class RequiredAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
        return value != null;
    }

    public override string Description => "Required field.";
}

// Validation for Prices
public class PositiveNumberAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return false;

        if (value is decimal dec)
            return dec > 0;
        
        return false;
    }

    public override string Description => "Value must be a positive number.";
}

// Validation for Dates
public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
        {
            return date > DateTime.Now;
        }
        return false;
    }

    public override string Description => "Must be a future date.";
}