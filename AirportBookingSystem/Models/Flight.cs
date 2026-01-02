public enum FlightClass
{
    Economy,
    Business,
    FirstClass
}

public class Flight
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string? DepartureCountry { get; set; }
    [Required]
    public string? DestinationCountry { get; set; }
    [FutureDate]
    public DateTime DepartureDate { get; set; }
    [Required]
    public string? DepartureAirport { get; set; }
    [Required]
    public string? ArrivalAirport { get; set; }
    [PositiveNumber]
    public decimal BasePrice { get; set; }
}