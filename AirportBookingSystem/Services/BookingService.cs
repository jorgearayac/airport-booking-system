public class BookingService
{
    private const string BookingDataFile = "Data/bookings.json";
    private readonly FlightService _flightService;

    public BookingService(FlightService flightService)
    {
        _flightService = flightService;
    }

    public List<Booking> GetAllBookings()
    {
        return FileStorage.LoadData<Booking>(BookingDataFile);
    }

    public Booking CreateBooking(
        Guid flightId,
        string passengerName,
        FlightClass flightClass
    )
    {
        var flights = _flightService.GetAllFlights();
        var flight = flights.FirstOrDefault(f => f.Id == flightId);

        if (flight == null)
            throw new InvalidOperationException("Flight not found");

        var finalPrice = PricingService.CalculateFinalPrice(
            flight.BasePrice,
            flightClass
        );

        var booking = new Booking
        {
            PassengerName = passengerName,
            FlightId = flight.Id,
            Class = flightClass,
            FinalPrice = finalPrice
        };

        var bookings = GetAllBookings();
        bookings.Add(booking);

        FileStorage.SaveData(BookingDataFile, bookings);

        return booking;
    }

    public bool CancelBooking(Guid bookingId)
    {
        var bookings = GetAllBookings();
        var booking = bookings.FirstOrDefault(b => b.Id == bookingId);

        if (booking == null)
            return false;

        bookings.Remove(booking);
        FileStorage.SaveData(BookingDataFile, bookings);
        return true;
    }

    public List<Booking> GetBookingsByPassenger(string passengerName)
    {
        var bookings = GetAllBookings();
        
        return bookings
            .Where(b => b.PassengerName?.Equals(passengerName, StringComparison.OrdinalIgnoreCase) == true)
            .ToList();
    }

    public Booking ModifyBooking(Guid bookingId, Guid? newFlightId, FlightClass? newClass, string? newPassengerName)
    {
        var bookings = GetAllBookings();
        var booking = bookings.FirstOrDefault(b => b.Id == bookingId);

        // Validate booking existence
        if (booking == null)
            throw new InvalidOperationException("Booking not found");

        // Update flight if newFlightId is provided
        if (newFlightId.HasValue)
        {
            var flights = _flightService.GetAllFlights();
            var flight = flights.FirstOrDefault(f => f.Id == newFlightId.Value);

            if (flight == null)
                throw new InvalidOperationException("New flight not found");

            booking.FlightId = flight.Id;

            // Recalculate price if class is also changing
            var flightClass = newClass ?? booking.Class;
            booking.FinalPrice = PricingService.CalculateFinalPrice(flight.BasePrice, flightClass);
        }

        // Update class if newClass is provided
        if (newClass.HasValue)
        {
            var flights = _flightService.GetAllFlights();
            var flight = flights.FirstOrDefault(f => f.Id == booking.FlightId);

            if (flight == null)
                throw new InvalidOperationException("Associated flight not found");

            booking.Class = newClass.Value;
            booking.FinalPrice = PricingService.CalculateFinalPrice(flight.BasePrice, newClass.Value);
        }

        if (!string.IsNullOrWhiteSpace(newPassengerName))
        {
            booking.PassengerName = newPassengerName;
        }

        FileStorage.SaveData(BookingDataFile, bookings);
        return booking;
    }
}