public class FlightService
{
    private const string FlightDataFile = "Data/flights.json";

    public List<Flight> GetAllFlights()
    {
        return FileStorage.LoadData<Flight>(FlightDataFile);
    }

    public void AddFlight(Flight flight)
    {
        var flights = GetAllFlights();
        flights.Add(flight);
        FileStorage.SaveData(FlightDataFile, flights);
    }

    public void SaveFlights(List<Flight> flights)
    {
        FileStorage.SaveData(FlightDataFile, flights);
    }
}