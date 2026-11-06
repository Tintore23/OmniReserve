namespace OmniReserve.Application.Reservations.DTOs;

public record ReservationDto(
    Guid Id,
    Guid RoomId,
    Guid UserId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice
);