using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Interfaces.Repository;

public interface IHallRepository
{
    Task Add(Hall hall, CancellationToken ct);
    Task<Hall> GetByIdAsync(int id, CancellationToken ct);
    Task<List<Hall>> GetAllAsync(CancellationToken ct);
    void Remove(Hall hall);
    
    Task<RoomService?> GetServiceByIdAsync(int serviceId, CancellationToken ct);
    Task AddServiceAsync(RoomService service, CancellationToken ct);
}