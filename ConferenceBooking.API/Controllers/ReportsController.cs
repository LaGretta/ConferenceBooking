using ConferenceBooking.Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.API.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue(
        [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct) =>
        Ok(await _reportService.GetRevenue(from, to, ct));
}