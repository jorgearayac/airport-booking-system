namespace AirportBookingSystem.UI
{
    using System;

    public class Passenger
    {
        public void PassengerMenu(FlightService flightService, BookingService bookingService)
        {
            while (true)
            {
                Console.WriteLine(" == Passenger Menu == ");
                Console.WriteLine();
                Console.WriteLine("1. Book a Flight");
                Console.WriteLine("2. View my Bookings");
                Console.WriteLine("3. Cancel a Booking");
                Console.WriteLine("4. Modify a Booking");
                Console.WriteLine("0. Exit");
                Console.WriteLine();
                Console.Write("Select an option: ");
                
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BookFlight(flightService, bookingService);
                        break;
                    case "2":
                        ViewBookings(bookingService);
                        break;
                    case "3":
                        CancelBookingUI(bookingService);
                        break;
                    case "4":
                        ModifyBookingUI(bookingService, flightService);
                        break;
                    case "0":
                        Console.WriteLine("Exiting the system. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }

                ConsoleHelp.Pause();
            }
        }

        // BOOK FLIGHT
        static void BookFlight(FlightService flightService, BookingService bookingService)
        {
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
            var flightClass = AskForFlightClass();

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
                var booking = bookingService.CreateBooking(flightId, passengerName, flightClass);
                Console.WriteLine("Booking successful!");
                Console.WriteLine($"Booking ID: {booking.Id} | Passenger: {booking.PassengerName} | Final Price: {booking.FinalPrice}");
                Console.WriteLine("Thank you for booking with us! :)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating booking: {ex.Message}");
            }
        }

        // VIEW BOOKINGS
        static void ViewBookings(BookingService bookingService)
        {
            Console.Write("Enter Passenger Name: ");
            var passengerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(passengerName))
            {
                Console.WriteLine("Passenger name cannot be empty.");
                return;
            }

            var bookings = bookingService.GetBookingsByPassenger(passengerName);
            
            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found for this passenger.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Bookings for {passengerName}:");

            foreach (var booking in bookings)
            {
                Console.WriteLine($"Booking ID: {booking.Id} | Flight ID: {booking.FlightId} | Class: {booking.Class} | Final Price: {booking.FinalPrice}");
            }
        }

        // CANCEL BOOKING
        static void CancelBookingUI(BookingService bookingService)
        {
            Console.Write("Enter Booking ID to cancel: ");
            var bookingIdInput = Console.ReadLine();

            if (!Guid.TryParse(bookingIdInput, out var bookingId))
            {
                Console.WriteLine("Invalid Booking ID, try another format");
                return;
            }

            var success = bookingService.CancelBooking(bookingId);
            if (success)
            {
                Console.WriteLine("Booking cancelled successfully");
            }
            else
            {
                Console.WriteLine("Booking not found");
            }
        }

        // MODIFY BOOKING
        static void ModifyBookingUI(BookingService bookingService, FlightService flightService)
        {
            Console.WriteLine("Enter your passenger name: ");
            var passengerName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(passengerName))
            {
                Console.WriteLine("Passenger name cannot be empty.");
                return;
            }

            Console.WriteLine("Enter Booking ID to modify: ");
            var bookingIdInput = Console.ReadLine();

            if (!Guid.TryParse(bookingIdInput, out var bookingId))
            {
                Console.WriteLine("Invalid Booking ID, try another format");
                return;
            }

            Guid? newFlightId = null;
            FlightClass? newFlightClass = null;
            string? newPassengerName = null;

            Console.WriteLine("Change Passenger? (y/n): ");

            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("New passenger name: ");
                newPassengerName = Console.ReadLine();
            }

            Console.WriteLine("Change Flight? (y/n): ");

            if (Console.ReadLine() == "y")
            {
                var flights = flightService.GetAllFlights();
                Console.WriteLine("Available Flights:");
                foreach (var flight in flights)
                {
                    Console.WriteLine($"{flight.Id} | {flight.DepartureCountry} to {flight.DestinationCountry} at {flight.DepartureDate} | Base Price: {flight.BasePrice}");
                }

                Console.WriteLine("Enter new Flight ID: ");
                var newFlightIdInput = Console.ReadLine();

                if (!Guid.TryParse(newFlightIdInput, out var parsedFlightId))
                {
                    Console.WriteLine("Invalid Flight ID.");
                    return;
                }

                newFlightId = parsedFlightId;
            }

            Console.WriteLine("Change Class? (y/n): ");

            if (Console.ReadLine() == "y")
            {
                newFlightClass = AskForFlightClass();
            }

            try
            {
                var updatedBooking = bookingService.ModifyBooking(
                    bookingId,
                    passengerName,
                    newFlightId,
                    newFlightClass,
                    newPassengerName
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // ASK FOR FLIGHT CLASS - Extracted Method
        static FlightClass AskForFlightClass()
        {
            while (true)
            {
                Console.WriteLine("Enter Flight Class: ");
                Console.WriteLine("1. Economy | 2. Business | 3. FirstClass");
                Console.Write("Choice: ");
                
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
                        Console.WriteLine("Invalid class selection. Please try again");
                        break;
                }
            }
        }
    }
}