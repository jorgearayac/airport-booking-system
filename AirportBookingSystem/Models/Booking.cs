public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? PassengerName { get; set; }
    public Guid FlightId { get; set; }
    public FlightClass Class { get; set; }
    public decimal FinalPrice { get; set; }
}