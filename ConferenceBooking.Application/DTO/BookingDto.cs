namespace ConferenceBooking.Application.DTO;

public class SearchHallsDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MinCapacity { get; set; }
}
public class CreateBookingDto
{
    public int HallId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<int> ServiceIds { get; set; } = new();
}
public class BookingResponseDto
{
    public int Id { get; set; }
    public int HallId { get; set; }
    public string HallName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal TotalCost { get; set; }
    public List<ServiceResponseDto> Services { get; set; } = new();
}