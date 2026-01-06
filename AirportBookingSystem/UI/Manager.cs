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
                Console.WriteLine("2. Import Flights from CSV");
                Console.WriteLine("3. Validate Flight Data");
                Console.WriteLine("0. Exit");
                Console.WriteLine();
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("TODO: Filtering bookings...");
                        break;
                    case "2":
                        Console.WriteLine("TODO: Import flights...");
                        break;
                    case "3":
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
                    $"Booking ID: {booking.Id} | Passenger: {booking.PassengerName} | Class: {booking.Class} | Price: {booking.FinalPrice}"
                );
            }
            
            ConsoleHelp.Pause();
        }
    }
}   