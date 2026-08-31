namespace ConferenceBooking.Application.DTO;

public class RevenueReportDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<HallRevenueDto> ByHall { get; set; } = new();
}

public class HallRevenueDto
{
    public int HallId { get; set; }
    public string HallName { get; set; } = string.Empty;
    public int BookingsCount { get; set; }
    public decimal Revenue { get; set; }
}