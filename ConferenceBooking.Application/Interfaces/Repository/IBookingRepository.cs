using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Interfaces.Repository;

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken ct);
    Task<Booking?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<Booking>> GetOverlappingAsync(int hallId, DateTime start, DateTime end, CancellationToken ct);
    Task<List<Booking>> GetAllAsync(CancellationToken ct);
}