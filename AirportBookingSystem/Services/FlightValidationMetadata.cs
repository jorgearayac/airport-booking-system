using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class FlightValidationMetadataService
{
    public List<ValidationDetail> GetValidationDetails()
    {
        var details = new List<ValidationDetail>();

        var properties = typeof(Flight).GetProperties();

        foreach (var prop in properties)
        {
            var detail = new ValidationDetail
            {
                PropertyName = prop.Name,
                Type = prop.PropertyType.Name
            };

            var attributes = prop.GetCustomAttributes();

            foreach (var attr in attributes)
            {
                switch (attr)
                {
                    case RequiredAttribute:
                        detail.Constraints.Add("Required");
                        break;

                    case RangeAttribute range:
                        detail.Constraints.Add(
                            $"Range: {range.Minimum} → {range.Maximum}");
                        break;

                    case FutureDateAttribute:
                        detail.Constraints.Add(
                            "Allowed Range: Today → Future");
                        break;
                }
            }

            if (detail.Constraints.Any())
                details.Add(detail);
        }

        return details;
    }
}
