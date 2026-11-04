namespace OmniReserve.Api.Models;

public record CreateReservationRequest(
    Guid RoomId,
    DateTime StartDate,
    DateTime EndDate
);