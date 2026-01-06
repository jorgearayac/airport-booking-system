namespace AirportBookingSystem.UI
{
    using System;
    using AirportBookingSystem.Models;

    public class Manager
    {
        public void ManagerMenu(FlightService flightService, BookingService bookingService)
        {
            while (true)
            {
                Console.WriteLine(" == Manager Menu == ");
                Console.WriteLine();
                Console.WriteLine("1. Filter Bookings");
                Console.WriteLine("2. Modify Bookings");
                Console.WriteLine("3. Cancel a Booking");
                Console.WriteLine("4. View all Bookings");
                Console.WriteLine("5. Import Flights");
                Console.WriteLine("0. Exit");
                Console.WriteLine();
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        FilterBookingsUI(bookingService);
                        break;
                    case "2":
                        ModifyBookingAsManagerUI(bookingService, flightService);
                        break;
                    case "3":
                        CancelBookingAsManagerUI(bookingService);
                        break;
                    case "4":
                        ViewAllBookings(bookingService);
                        break;
                    case "5":
                        Console.WriteLine("TODO: Validation metadata...");
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public static void FilterBookingsUI(BookingService bookingService)
        {
            Console.Clear();
            Console.WriteLine("== Filter Bookings ==");

            Console.Write("Enter Passenger Name (leave empty to ignore): ");
            var passengerName = Console.ReadLine();

            Console.Write("Flight class (1. Economy | 2. Business | 3. FirstClass || \n Leave empty to ignore): ");
            var classInput = Console.ReadLine();

            FlightClass? flightClass = classInput switch
            {
                "1" => FlightClass.Economy,
                "2" => FlightClass.Business,
                "3" => FlightClass.FirstClass,
                _ => null
            };

            Console.Write("Minimum price (leave empty to ignore): ");
            var minInput = Console.ReadLine();
            decimal? minPrice = decimal.TryParse(minInput, out var min) ? min : null;

            Console.Write("Maximum price (leave empty to ignore): ");
            var maxInput = Console.ReadLine();
            decimal? maxPrice = decimal.TryParse(maxInput, out var max) ? max : null;

            Console.WriteLine("Departure Country (leave empty to ignore): ");
            var departureCountry = Console.ReadLine();

            Console.WriteLine("Destination Country (leave empty to ignore): ");
            var destinationCountry = Console.ReadLine();

            Console.WriteLine("Departure Date [yyyy-mm-dd] (leave empty to ignore): ");
            var dateInput = Console.ReadLine();
            DateTime? departureDate = DateTime.TryParse(dateInput, out var date) ? date : null;

            Console.WriteLine("Departure Airport (leave empty to ignore): ");
            var departureAirport = Console.ReadLine();

            Console.WriteLine("Arrival Airport (leave empty to ignore): ");
            var arrivalAirport = Console.ReadLine();
            
            var results = bookingService.FilterBookings(
                passengerName,
                flightClass,
                minPrice,
                maxPrice,
                departureCountry,
                destinationCountry,
                departureAirport,
                arrivalAirport,
                departureDate);

            Console.WriteLine();
            Console.WriteLine($"Found {results.Count} bookings:");
            Console.WriteLine();

            foreach (var booking in results)
            {
                Console.WriteLine(
                    $"Booking ID: {booking.Id} | Passenger: {booking.PassengerName} | Class: {booking.Class} | Price: {booking.FinalPrice} | {departureCountry} -> {destinationCountry} | Date: {departureDate?.ToString("yyyy-MM-dd")}"
                );
            }
            
            ConsoleHelp.Pause();
        }

        static void ModifyBookingAsManagerUI(
            BookingService bookingService,
            FlightService flightService)
        {
            Console.Write("Enter Booking ID: ");
            if (!Guid.TryParse(Console.ReadLine(), out var bookingId))
            {
                Console.WriteLine("Invalid Booking ID");
                return;
            }

            Guid? newFlightId = null;
            FlightClass? newClass = null;
            string? newPassengerName = null;

            Console.Write("Change passenger name? (y/n): ");
            if (Console.ReadLine() == "y")
            {
                Console.Write("New passenger name: ");
                newPassengerName = Console.ReadLine();
            }

            Console.Write("Change flight? (y/n): ");
            if (Console.ReadLine() == "y")
            {
                var flights = flightService.GetAllFlights();

                foreach (var flight in flights)
                {
                    Console.WriteLine(
                        $"{flight.Id} | {flight.DepartureCountry} → {flight.DestinationCountry} | {flight.DepartureDate}");
                }

                Console.Write("New Flight ID: ");
                if (!Guid.TryParse(Console.ReadLine(), out var flightId))
                {
                    Console.WriteLine("Invalid Flight ID");
                    return;
                }

                newFlightId = flightId;
            }

            Console.Write("Change class? (y/n): ");
            if (Console.ReadLine() == "y")
            {
                newClass = AskForFlightClass();
            }

            try
            {
                var booking = bookingService.ModifyBookingAsManager(
                    bookingId,
                    newFlightId,
                    newClass,
                    newPassengerName
                );

                Console.WriteLine("Booking updated successfully:");
                Console.WriteLine(
                    $"Passenger: {booking.PassengerName} | Class: {booking.Class} | Price: {booking.FinalPrice}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static FlightClass AskForFlightClass()
        {
            while (true)
            {
                Console.Write("Select Class (1. Economy | 2. Business | 3. FirstClass): ");
                var classInput = Console.ReadLine();

                switch (classInput)
                {
                    case "1":
                        return FlightClass.Economy;
                    case "2":
                        return FlightClass.Business;
                    case "3":
                        return FlightClass.FirstClass;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
        }

        static void CancelBookingAsManagerUI(BookingService bookingService)
        {
            Console.Write("Enter Booking ID to cancel: ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out var bookingId))
            {
                Console.WriteLine("Invalid Booking ID.");
                return;
            }

            var success = bookingService.CancelBooking(bookingId);

            if (success)
            {
                Console.WriteLine("Booking cancelled successfully.");
            }
            else
            {
                Console.WriteLine("Booking not found.");
            }
        }

        static void ViewAllBookings(BookingService bookingService)
        {
            var bookings = bookingService.GetAllBookings();

            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            Console.WriteLine("\n--- All Bookings ---");

            foreach (var booking in bookings)
            {
                Console.WriteLine(
                    $"ID: {booking.Id} | " +
                    $"Passenger: {booking.PassengerName} | " +
                    $"Flight: {booking.FlightId} | " +
                    $"Price: {booking.FinalPrice} | " +
                    $"Class: {booking.Class}"
                );
            }
        }
    }
}   