using MediatR;
using OmniReserve.Application.Reservations.DTOs; 

namespace OmniReserve.Application.Reservations.Queries.GetAllReservations;

public record GetAllReservationsQuery() : IRequest<List<ReservationDto>>;