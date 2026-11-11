using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting; // 1. Agrega este using
using OmniReserve.Application.Rooms.Commands.CreateRoom;
using OmniReserve.Application.Rooms.Queries.GetRoomById;

namespace OmniReserve.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GlobalLimit")] // 2. Aplica la política global a todo el controlador
public class RoomsController : ControllerBase
{
    private readonly ISender _sender;

    public RoomsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(Guid id)
    {
        var query = new GetRoomByIdQuery(id); 
        var result = await _sender.Send(query);
        return Ok(result);
    }
}