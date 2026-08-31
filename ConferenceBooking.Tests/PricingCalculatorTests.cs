using ConferenceBooking.Application.Pricing;
using FluentAssertions;
using Xunit;

namespace ConferenceBooking.Tests;

public class PricingCalculatorTests
{
    private readonly PricingCalculator _calculator = new();

    // коефіцієнти по годинах
    [Theory]
    [InlineData(6, 0.90)]   // ранок
    [InlineData(7, 0.90)]
    [InlineData(8, 0.90)]
    [InlineData(9, 1.00)]   // стандарт
    [InlineData(10, 1.00)]
    [InlineData(11, 1.00)]
    [InlineData(12, 1.15)]  // пік
    [InlineData(13, 1.15)]  // пік
    [InlineData(14, 1.00)]  // стандарт
    [InlineData(17, 1.00)]
    [InlineData(18, 0.80)]  // вечір
    [InlineData(22, 0.80)]
    public void GetHourCoefficient_ReturnsCorrectCoefficient(int hour, decimal expected)
    {
        _calculator.GetHourCoefficient(hour).Should().Be(expected);
    }

    // розрахунок вартості оренди
    [Fact]
    public void CalculateHallCost_StandardHours_ReturnsBaseTimesHours()
    {
        var start = new DateTime(2026, 9, 1, 10, 0, 0);
        var end = new DateTime(2026, 9, 1, 12, 0, 0);

        var cost = _calculator.CalculateHallCost(2000, start, end);

        cost.Should().Be(4000);  
    }
    [Fact]
    public void CalculateHallCost_CrossingPeakHours_AppliesPeakSurcharge()
    {
        var start = new DateTime(2026, 9, 1, 10, 0, 0);
        var end = new DateTime(2026, 9, 1, 14, 0, 0);

        var cost = _calculator.CalculateHallCost(2000, start, end);

        cost.Should().Be(8600); 
    }
    [Fact]
    public void CalculateHallCost_MorningHours_AppliesDiscount()
    {
        var start = new DateTime(2026, 9, 1, 6, 0, 0);
        var end = new DateTime(2026, 9, 1, 9, 0, 0);

        var cost = _calculator.CalculateHallCost(2000, start, end);

        cost.Should().Be(5400);  
    }
    [Fact]
    public void CalculateHallCost_EveningHours_AppliesDiscount()
    {
        var start = new DateTime(2026, 9, 1, 18, 0, 0);
        var end = new DateTime(2026, 9, 1, 20, 0, 0);

        var cost = _calculator.CalculateHallCost(2000, start, end);

        cost.Should().Be(3200);  
    }
    [Fact]
    public void CalculateHallCost_SpanningMultiplePeriods_CalculatesEachHour()
    {
        var start = new DateTime(2026, 9, 1, 8, 0, 0);
        var end = new DateTime(2026, 9, 1, 10, 0, 0);

        var cost = _calculator.CalculateHallCost(2000, start, end);

        cost.Should().Be(3800);   
    }
}