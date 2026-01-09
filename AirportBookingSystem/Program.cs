class Program
{
    static void Main()
    {
        var flightService = new FlightService();
        var bookingService = new BookingService(flightService);

        while (true)
        {
            Console.WriteLine("==== Airport Ticket Booking System ====");
            Console.WriteLine();
            Console.WriteLine("1. Passenger");
            Console.WriteLine("2. Manager");
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("Select your role: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var passenger = new AirportBookingSystem.UI.Passenger();
                    passenger.PassengerMenu(flightService, bookingService);
                    break;
                case "2":
                    var manager = new AirportBookingSystem.UI.Manager();
                    manager.ManagerMenu(flightService, bookingService);
                    break;
                case "0":
                    Console.WriteLine("Exiting the system. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }
}