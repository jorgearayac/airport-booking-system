public class FlightImportService
{
    private readonly FlightService _flightService;

    public FlightImportService(FlightService flightService)
    {
        _flightService = flightService;
    }

    public List<string> ImportFlights(List<Flight> flightsToImport)
    {
        var errors = new List<string>();
        
        foreach (var flight in flightsToImport)
        {
            var validationResult = ObjectValidator.Validate(flight);
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join("; ", validationResult.Errors.Select(e => $"{e.PropertyName}: {e.Message}"));
                errors.Add($"Flight {flight.Id} has validation errors: {errorMessages}");
                
                // Skip invalid flights
                continue;
            }

            // Only add valid flights
            _flightService.AddFlight(flight);
        }

        return errors;
    }
}