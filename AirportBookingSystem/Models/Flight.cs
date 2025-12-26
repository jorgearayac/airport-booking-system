public enum FlightClass
{
    Economy,
    Business,
    FirstClass
}

public class Flight
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? DepartureCountry { get; set; }
    public string? DestinationCountry { get; set; }
    public DateTime DepartureDate { get; set; }
    public string? DepartureAirport { get; set; }
    public string? ArrivalAirport { get; set; }
    public decimal BasePrice { get; set; }
}