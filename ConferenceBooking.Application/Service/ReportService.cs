using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Interfaces.Repository;
using ConferenceBooking.Application.Interfaces.Service;

namespace ConferenceBooking.Application.Service;

public class ReportService : IReportService
{
    private readonly IBookingRepository _bookingRepository;

    public ReportService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<RevenueReportDto> GetRevenue(DateTime from, DateTime to, CancellationToken ct)
    {
        var bookings = await _bookingRepository.GetAllAsync(ct);

        // бронювання у заданому періоді за часом початку
        var inPeriod = bookings
            .Where(b => b.StartTime >= from && b.StartTime < to)
            .ToList();

        var byHall = inPeriod
            .GroupBy(b => new { b.HallId, b.Hall.Name })
            .Select(g => new HallRevenueDto
            {
                HallId = g.Key.HallId,
                HallName = g.Key.Name,
                BookingsCount = g.Count(),
                Revenue = g.Sum(b => b.TotalCost)
            })
            .OrderByDescending(h => h.Revenue)
            .ToList();

        return new RevenueReportDto
        {
            From = from,
            To = to,
            TotalRevenue = inPeriod.Sum(b => b.TotalCost),
            ByHall = byHall
        };
    }
}