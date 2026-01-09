using System.ComponentModel.DataAnnotations;

public enum FlightClass
{
    Economy,
    Business,
    FirstClass
}

public class Flight
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string? DepartureCountry { get; set; }

    [Required]
    public string? DestinationCountry { get; set; }

    [Required]
    [FutureDate]
    public DateTime DepartureDate { get; set; }

    [Required]
    public string? DepartureAirport { get; set; }

    [Required]
    public string? ArrivalAirport { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    [PositiveNumber]
    public decimal BasePrice { get; set; }
}