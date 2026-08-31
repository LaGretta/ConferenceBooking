namespace ConferenceBooking.Application.DTO;

public class CreateServiceDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class ServiceResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}