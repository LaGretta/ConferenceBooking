using ConferenceBooking.Application.Interfaces.Repository;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Booking booking, CancellationToken ct) =>
        await _context.Bookings.AddAsync(booking, ct);

    public async Task<Booking?> GetByIdAsync(int id, CancellationToken ct) =>
        await _context.Bookings
            .Include(b => b.Hall)
            .Include(b => b.BookingServices)
            .ThenInclude(bs => bs.RoomService)
            .FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<List<Booking>> GetOverlappingAsync(
        int hallId, DateTime start, DateTime end, CancellationToken ct) =>
        await _context.Bookings
            .Where(b => b.HallId == hallId
                        && b.StartTime < end
                        && b.EndTime > start)
            .ToListAsync(ct);

    public async Task<List<Booking>> GetAllAsync(CancellationToken ct) =>
        await _context.Bookings
            .Include(b => b.Hall)
            .ToListAsync(ct);
}