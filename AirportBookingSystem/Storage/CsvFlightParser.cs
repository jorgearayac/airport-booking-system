using System.Globalization;

public static class CsvFlightParser
{
    public static List<Flight> Parse(string filePath)
    {
        var flights = new List<Flight>();

        if (!File.Exists(filePath))
            throw new FileNotFoundException("CSV file not found.");

        var lines = File.ReadAllLines(filePath);

        // Skip header
        for (int i = 1; i < lines.Length; i++)
        {
            var columns = lines[i].Split(',');

            var flight = new Flight
            {
                DepartureCountry = columns[0],
                DestinationCountry = columns[1],
                DepartureDate = DateTime.Parse(columns[2], CultureInfo.InvariantCulture),
                DepartureAirport = columns[3],
                ArrivalAirport = columns[4],
                BasePrice = decimal.Parse(columns[5], CultureInfo.InvariantCulture)
            };

            flights.Add(flight);
        }

        return flights;
    }
}