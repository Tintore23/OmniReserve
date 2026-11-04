using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Application.Reservations.DTOs; 
namespace OmniReserve.Application.Reservations.Queries.GetAllReservations;

public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, List<ReservationDto>>
{
    private readonly IReservationRepository _reservationRepository;

    public GetAllReservationsQueryHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<List<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
    {
        // Obtener todas las reservaciones del repositorio
        var reservations = await _reservationRepository.GetAllAsync();

        // Mapear la entidad a DTO
        return reservations.Select(r => new ReservationDto(
            r.Id,
            r.RoomId,
            r.UserId,
            r.CheckInDate,
            r.CheckOutDate,
            r.TotalPrice
        )).ToList();
    }
}