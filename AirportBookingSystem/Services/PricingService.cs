public static class PricingService
{
    public static decimal CalculateFinalPrice(decimal basePrice, FlightClass flightClass)
    {
        return flightClass switch
        {
            FlightClass.Economy => basePrice,
            FlightClass.Business => basePrice * 1.5m,
            FlightClass.FirstClass => basePrice * 2.0m,
            _ => basePrice
        };
    }
}