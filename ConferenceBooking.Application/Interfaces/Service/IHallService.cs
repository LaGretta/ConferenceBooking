using ConferenceBooking.Application.DTO;

namespace ConferenceBooking.Application.Interfaces.Service;

public interface IHallService
{
    Task<HallResponseDto> Create(CreateHallDto dto, CancellationToken ct);
    Task<HallResponseDto> Update(int id, UpdateHallDto dto, CancellationToken ct);
    Task Delete(int id, CancellationToken ct);
    Task<ServiceResponseDto> AddService(int hallId, CreateServiceDto dto, CancellationToken ct);
    Task<List<HallResponseDto>> GetAll(CancellationToken ct);
    Task<HallResponseDto> GetById(int id, CancellationToken ct);
}