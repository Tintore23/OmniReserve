using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Api.Models; // <-- Asegúrate de incluir el using de tu request
using OmniReserve.Application.Reservations.Commands.CreateReservation; // <-- Namespace del comando

namespace OmniReserve.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
    {
        // 1. Extracción segura del ID de usuario desde el JWT
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Token inválido o usuario no identificado." });
        }

        // 2. Orquestar el comando usando los nombres exactos de propiedades del comando
        var command = new CreateReservationCommand(
            UserId: userId,
            RoomId: request.RoomId,
            CheckInDate: request.StartDate,   // <-- Coincide con CheckInDate del command
            CheckOutDate: request.EndDate,    // <-- Coincide con CheckOutDate del command
            TotalPrice: 0 // (O el valor correspondiente si tu comando lo requiere)
        );

        var reservationId = await _mediator.Send(command);

        return Ok(reservationId);
    }
}