using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IApplicationDbContext _context;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepository,
        IApplicationDbContext context)
    {
        _reservationRepository = reservationRepository;
        _context = context;
    }

    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        // 1. Evaluar la regla invariante de disponibilidad (Prevenir Double-Booking)
        bool isAvailable = await _reservationRepository.IsRoomAvailableAsync(
            request.RoomId,
            request.CheckInDate, // (O request.StartDate según los nombres de tu Command)
            request.CheckOutDate  // (O request.EndDate según los nombres de tu Command)
        );

        if (!isAvailable)
        {
            throw new Exception("La habitación seleccionada ya está ocupada en el rango de fechas solicitado.");
        }

        // 2. Instanciar la entidad Reservation si la validación es exitosa
        var reservation = new Reservation(
            request.UserId,
            request.RoomId,
            request.CheckInDate,
            request.CheckOutDate,
            request.TotalPrice
        );

        // 3. Agregar al repositorio y confirmar los cambios en la base de datos
        await _reservationRepository.AddAsync(reservation);
        await _context.SaveChangesAsync(cancellationToken);

        // 4. Retornar el ID de la nueva reservación creada
        return reservation.Id;
    }
}