using ConferenceBooking.Application.DTO;
using ConferenceBooking.Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.API.Controllers;

[ApiController]
[Route("api/halls")]
public class HallsController : ControllerBase
{
    private readonly IHallService _hallService;

    public HallsController(IHallService hallService)
    {
        _hallService = hallService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateHallDto dto, CancellationToken ct) =>
        Ok(await _hallService.Create(dto, ct));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateHallDto dto, CancellationToken ct) =>
        Ok(await _hallService.Update(id, dto, ct));
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _hallService.Delete(id, ct);
        return NoContent();
    }
    [HttpPost("{id}/services")]
    public async Task<IActionResult> AddService(int id, CreateServiceDto dto, CancellationToken ct) =>
        Ok(await _hallService.AddService(id, dto, ct));
}