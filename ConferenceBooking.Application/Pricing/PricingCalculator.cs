namespace ConferenceBooking.Application.Pricing;

public class PricingCalculator
{
    public decimal GetHourCoefficient(int hour)
    {
        if (hour is 12 or 13)
            return 1.15m;

        if (hour is >= 6 and < 9)
            return 0.90m;

        if (hour is >= 9 and < 18)
            return 1.00m;

        if (hour is >= 18 and < 23)
            return 0.80m;

        return 1.00m;
    }

    // вартість оренди залу без послуг — погодинно
    public decimal CalculateHallCost(decimal basePricePerHour, DateTime start, DateTime end)
    {
        decimal total = 0m;

        for (var hourStart = start; hourStart < end; hourStart = hourStart.AddHours(1))
        {
            total += basePricePerHour * GetHourCoefficient(hourStart.Hour);
        }
        return total;
    }
}