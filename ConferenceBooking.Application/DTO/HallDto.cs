namespace ConferenceBooking.Application.DTO;

public class CreateHallDto
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePricePerHour { get; set; }
    public List<CreateServiceDto> Services { get; set; } = new();
}

public class UpdateHallDto
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePricePerHour { get; set; }
}

public class HallResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePricePerHour { get; set; }
    public List<ServiceResponseDto> Services { get; set; } = new();
}