using ConferenceBooking.Application.DTO;

namespace ConferenceBooking.Application.Interfaces.Service;

public interface IReportService
{
    Task<RevenueReportDto> GetRevenue(DateTime from, DateTime to, CancellationToken ct);
}