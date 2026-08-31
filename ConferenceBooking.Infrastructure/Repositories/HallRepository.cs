using ConferenceBooking.Application.Interfaces.Repository;
using ConferenceBooking.Domain.Entities;
using ConferenceBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Repositories;

public class HallRepository : IHallRepository
{
    private readonly AppDbContext _context;

    public HallRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(Hall hall, CancellationToken ct) =>
        await _context.Halls.AddAsync(hall, ct);
    
    public async Task<Hall?> GetByIdAsync(int id, CancellationToken ct) =>
        await _context.Halls
            .Include(h => h.Services)
            .FirstOrDefaultAsync(h => h.Id == id, ct);

    public async Task<List<Hall>> GetAllAsync(CancellationToken ct) =>
        await _context.Halls
            .Include(h => h.Services)
            .ToListAsync(ct);

    public void Remove(Hall hall) =>
        _context.Halls.Remove(hall);

    public async Task<RoomService?> GetServiceByIdAsync(int serviceId, CancellationToken ct) =>
        await _context.RoomServices.FirstOrDefaultAsync(s => s.Id == serviceId, ct);

    public async Task AddServiceAsync(RoomService service, CancellationToken ct) =>
        await _context.RoomServices.AddAsync(service, ct);
}