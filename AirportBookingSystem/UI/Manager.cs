namespace AirportBookingSystem.UI
{
    using System;

    public class Manager
    {
        public void ShowManagerMenu()
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
                        // Logic to filter bookings
                        break;
                    case "2":
                        // Logic to import flights
                        break;
                    case "3":
                        // Logic to validate
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}