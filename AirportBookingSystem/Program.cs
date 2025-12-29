class Program
{
    static void Main()
    {
        var flightService = new FlightService();
        var bookingService = new BookingService(flightService);

        Console.WriteLine("==== Airport Ticket Booking System ====");
        Console.WriteLine();

        // List flights
        var flights = flightService.GetAllFlights();

        if (flights.Count == 0)
        {
            Console.WriteLine("No flights available :c");
            return;
        }
        
        Console.WriteLine("Available Flights:");
        foreach (var flight in flights)
        {
            Console.WriteLine($"{flight.Id} | {flight.DepartureCountry} to {flight.DestinationCountry} at {flight.DepartureDate} | Base Price: {flight.BasePrice}");
        }

        Console.WriteLine();

        // Ask for flight ID
        Console.Write("Enter Flight ID to book: ");
        var flightIdInput = Console.ReadLine();

        if (!Guid.TryParse(flightIdInput, out var flightId))
        {
            Console.WriteLine("Invalid Flight ID T.T, try another format");
            return;
        }

        // Ask for Flight Class
        Console.WriteLine("Enter Flight Class: ");
        Console.WriteLine("1. Economy | 2. Business | 3. FirstClass");
        Console.Write("Choice: ");
        var classInput = Console.ReadLine();
        
        FlightClass flightClass = classInput switch
        {
            "1" => FlightClass.Economy,
            "2" => FlightClass.Business,
            "3" => FlightClass.FirstClass,
            _ => throw new InvalidOperationException("Invalid class selection")
        };

        // Ask for Passenger Name
        Console.Write("Enter Passenger Name: ");
        var passengerName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(passengerName))
        {
            Console.WriteLine("Passenger name cannot be empty U.U");
            return;
        }

        // Create Booking
        try
        {
            var booking = bookingService.CreateBooking(flightId, passengerName!, flightClass);
            Console.WriteLine("Booking successful!");
            Console.WriteLine($"Booking ID: {booking.Id} | Passenger: {booking.PassengerName} | Final Price: {booking.FinalPrice}");
            Console.WriteLine("Thank you for booking with us! :)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating booking: {ex.Message}");
        }
    }
}