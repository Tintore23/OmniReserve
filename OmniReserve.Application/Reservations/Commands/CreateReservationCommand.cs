using System;
using MediatR;

namespace OmniReserve.Application.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(
    Guid UserId,
    Guid RoomId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice
) : IRequest<Guid>;