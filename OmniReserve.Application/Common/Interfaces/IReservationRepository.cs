using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Common.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetReservationWithDetailsAsync(Guid id);
    Task<List<Reservation>> GetAllAsync();
    Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime start, DateTime end);
    Task AddAsync(Reservation reservation);
}