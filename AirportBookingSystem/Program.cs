var flights = FileStorage.LoadData<Flight>("Data/flights.json");

if (flights.Count == 0)
{
    Console.WriteLine("No flights available.");
}
else
{
    foreach (var flight in flights)
    {
        Console.WriteLine($"{flight.Id} | {flight.DepartureCountry} → {flight.DestinationCountry} | {flight.DepartureDate} | ${flight.BasePrice}");
    }
}
