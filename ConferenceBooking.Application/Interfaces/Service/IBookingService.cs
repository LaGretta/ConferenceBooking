using ConferenceBooking.Application.DTO;

namespace ConferenceBooking.Application.Interfaces.Service;

public interface IBookingService
{
    Task<List<HallResponseDto>> FindAvailableHalls(SearchHallsDto dto, CancellationToken ct);
    Task<BookingResponseDto> CreateBooking(CreateBookingDto dto, CancellationToken ct);
}