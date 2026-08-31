namespace ConferenceBooking.Domain.Entities;

public class BookingServiceItem
{
    public int Id { get; set; }
    
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    
    public int RoomServiceId  { get; set; }
    public RoomService RoomService { get; set; } = null!;
}