namespace AirportBookingSystem.Models;

public class BookingFilter
{
    public Guid? FlightId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? DepartureCountry { get; set; }
    public string? DestinationCountry { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? DepartureAirport { get; set; }
    public string? ArrivalAirport { get; set; }
    public string? PassengerName { get; set; }
    public FlightClass? Class { get; set; }
}