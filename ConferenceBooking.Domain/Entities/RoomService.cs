namespace ConferenceBooking.Domain.Entities;

public class RoomService
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    public int HallId { get; set; }
    public Hall Hall { get; set; } = null!;
}