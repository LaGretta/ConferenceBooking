using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("search")]
    public async Task<IActionResult> FindAvailable(SearchHallsDto dto, CancellationToken ct) =>
        Ok(await _bookingService.FindAvailableHalls(dto, ct));
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingDto dto, CancellationToken ct) =>
        Ok(await _bookingService.CreateBooking(dto, ct));
}