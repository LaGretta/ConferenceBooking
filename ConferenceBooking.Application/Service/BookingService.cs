using AutoMapper;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Interfaces.Repository;
using ConferenceBooking.Application.Interfaces.Service;
using ConferenceBooking.Application.Pricing;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Service;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHallRepository _hallRepository;
    private readonly PricingCalculator _pricing;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingService(
        IBookingRepository bookingRepository,
        IHallRepository hallRepository,
        PricingCalculator pricing,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _hallRepository = hallRepository;
        _pricing = pricing;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<HallResponseDto>> FindAvailableHalls(SearchHallsDto dto, CancellationToken ct)
    {
        if (dto.EndTime <= dto.StartTime)
            throw new InvalidOperationException("EndTime must be after StartTime");

        var allHalls = await _hallRepository.GetAllAsync(ct);

        var available = new List<Hall>();
        foreach (var hall in allHalls)
        {
            if (hall.Capacity < dto.MinCapacity)
                continue;
            var overlapping = await _bookingRepository
                .GetOverlappingAsync(hall.Id, dto.StartTime, dto.EndTime, ct);

            if (overlapping.Count == 0)
                available.Add(hall);
        }

        return _mapper.Map<List<HallResponseDto>>(available);
    }

    public async Task<BookingResponseDto> CreateBooking(CreateBookingDto dto, CancellationToken ct)
    {
        if (dto.EndTime <= dto.StartTime)
            throw new InvalidOperationException("EndTime must be after StartTime");
        if (dto.StartTime < DateTime.UtcNow)
            throw new InvalidOperationException("Cannot book in the past");

        var hall = await _hallRepository.GetByIdAsync(dto.HallId, ct);
        if (hall == null)
            throw new KeyNotFoundException("Hall not found");

        var overlapping = await _bookingRepository
            .GetOverlappingAsync(dto.HallId, dto.StartTime, dto.EndTime, ct);
        if (overlapping.Count > 0)
            throw new InvalidOperationException("Hall is already booked for this time");

        var chosenServices = hall.Services
            .Where(s => dto.ServiceIds.Contains(s.Id))
            .ToList();
        if (chosenServices.Count != dto.ServiceIds.Count)
            throw new InvalidOperationException("Some services do not belong to this hall");

        var hallCost = _pricing.CalculateHallCost(hall.BasePricePerHour, dto.StartTime, dto.EndTime);
        var servicesCost = chosenServices.Sum(s => s.Price);
        var total = hallCost + servicesCost;

        var booking = new Booking
        {
            HallId = dto.HallId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            TotalCost = total,
            CreatedAt = DateTime.UtcNow,
            BookingServices = chosenServices
                .Select(s => new BookingServiceItem { RoomServiceId = s.Id })
                .ToList()
        };

        await _bookingRepository.AddAsync(booking, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new BookingResponseDto
        {
            Id = booking.Id,
            HallId = hall.Id,
            HallName = hall.Name,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            TotalCost = total,
            Services = _mapper.Map<List<ServiceResponseDto>>(chosenServices)
        };
    }
}