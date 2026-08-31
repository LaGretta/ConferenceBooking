using AutoMapper;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Interfaces.Repository;
using ConferenceBooking.Application.Interfaces.Service;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Service;

public class HallService : IHallService
{
    private readonly IHallRepository _hallRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HallService(
        IHallRepository hallRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _hallRepository = hallRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<HallResponseDto> Create(CreateHallDto dto, CancellationToken ct)
    {
        var hall = new Hall
        {
            Name = dto.Name,
            Capacity = dto.Capacity,
            BasePricePerHour = dto.BasePricePerHour,
            Services = dto.Services.Select(s => new RoomService
            {
                Name = s.Name,
                Price = s.Price
            }).ToList()
        };

        await _hallRepository.Add(hall, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<HallResponseDto>(hall);
    }

    public async Task<HallResponseDto> Update(int id, UpdateHallDto dto, CancellationToken ct)
    {
        var hall = await _hallRepository.GetByIdAsync(id, ct);
        if (hall == null)
            throw new KeyNotFoundException("Hall not found");

        hall.Name = dto.Name;
        hall.Capacity = dto.Capacity;
        hall.BasePricePerHour = dto.BasePricePerHour;

        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<HallResponseDto>(hall);
    }

    public async Task Delete(int id, CancellationToken ct)
    {
        var hall = await _hallRepository.GetByIdAsync(id, ct);
        if (hall == null)
            throw new KeyNotFoundException("Hall not found");

        _hallRepository.Remove(hall);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<ServiceResponseDto> AddService(int hallId, CreateServiceDto dto, CancellationToken ct)
    {
        var hall = await _hallRepository.GetByIdAsync(hallId, ct);
        if (hall == null)
            throw new KeyNotFoundException("Hall not found");

        var service = new RoomService
        {
            Name = dto.Name,
            Price = dto.Price,
            HallId = hallId
        };

        await _hallRepository.AddServiceAsync(service, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<ServiceResponseDto>(service);
    }
}