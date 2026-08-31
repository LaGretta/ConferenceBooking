using AutoMapper;
using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Application.Interfaces.Repository;
using ConferenceBooking.Application.Mapping;
using ConferenceBooking.Application.Pricing;
using ConferenceBooking.Application.Service;
using ConferenceBooking.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ConferenceBooking.Tests;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepo = new();
    private readonly Mock<IHallRepository> _hallRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly IMapper _mapper;
    private readonly BookingService _sut;

    public BookingServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<HallMapping>();
            cfg.AddProfile<BookingMapping>();
        }, new LoggerFactory());
        _mapper = config.CreateMapper();
        
        _sut = new BookingService(
            _bookingRepo.Object,
            _hallRepo.Object,
            new PricingCalculator(),
            _uow.Object,
            _mapper);
    }

    private static Hall SampleHall() => new()
    {
        Id = 1,
        Name = "Зал А",
        Capacity = 50,
        BasePricePerHour = 2000,
        Services = new List<RoomService>
        {
            new() { Id = 1, Name = "Проєктор", Price = 500, HallId = 1 },
            new() { Id = 2, Name = "Wi-Fi", Price = 300, HallId = 1 }
        }
    };

    [Fact]
    public async Task CreateBooking_ValidRequest_CalculatesCostAndSaves()
    {
        var hall = SampleHall();
        _hallRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(hall);
        _bookingRepo.Setup(r => r.GetOverlappingAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Booking>());

        var dto = new CreateBookingDto
        {
            HallId = 1,
            StartTime = new DateTime(2099, 9, 1, 10, 0, 0),
            EndTime = new DateTime(2099, 9, 1, 14, 0, 0),
            ServiceIds = new List<int> { 1, 2 }
        };

        var result = await _sut.CreateBooking(dto, CancellationToken.None);
        result.TotalCost.Should().Be(9400);
        result.HallName.Should().Be("Зал А");
        result.Services.Should().HaveCount(2);
        _uow.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateBooking_HallNotFound_ThrowsKeyNotFound()
    {
        _hallRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Hall?)null);

        var dto = new CreateBookingDto
        {
            HallId = 999,
            StartTime = new DateTime(2099, 9, 1, 10, 0, 0),
            EndTime = new DateTime(2099, 9, 1, 12, 0, 0)
        };

        var act = () => _sut.CreateBooking(dto, CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task CreateBooking_InThePast_Throws()
    {
        var dto = new CreateBookingDto
        {
            HallId = 1,
            StartTime = new DateTime(2020, 1, 1, 10, 0, 0),
            EndTime = new DateTime(2020, 1, 1, 12, 0, 0)
        };

        var act = () => _sut.CreateBooking(dto, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Cannot book in the past");
    }

    [Fact]
    public async Task CreateBooking_ForeignService_Throws()
    {
        var hall = SampleHall();
        _hallRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(hall);

        var dto = new CreateBookingDto
        {
            HallId = 1,
            StartTime = new DateTime(2099, 9, 1, 10, 0, 0),
            EndTime = new DateTime(2099, 9, 1, 12, 0, 0),
            ServiceIds = new List<int> { 99 }   
        };

        var act = () => _sut.CreateBooking(dto, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("*do not belong*");
    }

    [Fact]
    public async Task CreateBooking_AlreadyBooked_Throws()
    {
        var hall = SampleHall();
        _hallRepo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(hall);
        _bookingRepo.Setup(r => r.GetOverlappingAsync(1, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Booking> { new() { Id = 5 } });

        var dto = new CreateBookingDto
        {
            HallId = 1,
            StartTime = new DateTime(2099, 9, 1, 10, 0, 0),
            EndTime = new DateTime(2099, 9, 1, 12, 0, 0)
        };

        var act =  () => _sut.CreateBooking(dto, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("*already booked*");
        _uow.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindAvailableHalls_FiltersByCapacityAndAvailability()
    {
        var halls = new List<Hall>
        {
            new() { Id = 1, Name = "А", Capacity = 50, BasePricePerHour = 2000, Services = new() },
            new() { Id = 2, Name = "B", Capacity = 100, BasePricePerHour = 3500, Services = new() }
        };
        _hallRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(halls);
        _bookingRepo.Setup(r => r.GetOverlappingAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<Booking>());

        var dto = new SearchHallsDto
        {
            StartTime = new DateTime(2099, 9, 1, 10, 0, 0),
            EndTime = new DateTime(2099, 9, 1, 12, 0, 0),
            MinCapacity = 60 
        };
        var result = await _sut.FindAvailableHalls(dto, CancellationToken.None);
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("B");
    }
}