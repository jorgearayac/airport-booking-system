using AirportBookingSystem.Models;

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

    public Booking ModifyBooking(Guid bookingId, string passengerName, Guid? newFlightId, FlightClass? newClass, string? newPassengerName)
    {
        var bookings = GetAllBookings();
        var booking = bookings.FirstOrDefault(b => b.Id == bookingId);

        // Validate booking existence
        if (booking == null)
            throw new InvalidOperationException("Booking not found");

        if (!booking.PassengerName!.Equals(passengerName, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Passenger name does not match the booking record\nYou can only modify your own bookings");

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

    public List<Booking> FilterBookings(
        string? passengerName,
        FlightClass? flightClass,
        decimal? minPrice,
        decimal? maxPrice,
        string? departureCountry,
        string? destinationCountry,
        string? departureAirport,
        string? arrivalAirport,
        DateTime? departureDate)
    {
        var bookings = GetAllBookings();
        var flights = _flightService.GetAllFlights();

        var query = from booking in bookings
                    join flight in flights on booking.FlightId equals flight.Id
                    select new { booking, flight };

        if (!string.IsNullOrWhiteSpace(passengerName))
        {
            query = query.Where(q => 
                q.booking.PassengerName != null &&
                q.booking.PassengerName!.Equals(passengerName, StringComparison.OrdinalIgnoreCase));
        }

        if (flightClass.HasValue)
        {
            query = query.Where(q => q.booking.Class == flightClass.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(q => q.booking.FinalPrice >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(q => q.booking.FinalPrice <= maxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(departureCountry))
        {
            query = query.Where(q => 
                q.flight.DepartureCountry != null &&
                q.flight.DepartureCountry!.Equals(departureCountry, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrWhiteSpace(destinationCountry))
        {
            query = query.Where(q => 
                q.flight.DestinationCountry != null &&
                q.flight.DestinationCountry!.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(departureAirport))
        {
            query = query.Where(q => 
                q.flight.DepartureAirport != null &&
                q.flight.DepartureAirport!.Equals(departureAirport, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(arrivalAirport))
        {
            query = query.Where(q => 
                q.flight.ArrivalAirport != null &&
                q.flight.ArrivalAirport!.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase));
        }

        if (departureDate.HasValue)
        {
            query = query.Where(q => q.flight.DepartureDate.Date == departureDate.Value.Date);
        }

        return query.Select(q => q.booking).ToList();
    }
}