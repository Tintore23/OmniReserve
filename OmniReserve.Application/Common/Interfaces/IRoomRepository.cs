using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Common.Interfaces;

public interface IRoomRepository
{
    Task<List<Room>> GetAllAsync(CancellationToken cancellationToken = default); // <-- Agrega el parámetro aquí

    Task AddAsync(Room room);

    Task<Room?> GetByIdAsync(Guid id);

    Task<Room?> SearchByNumberAsync(string roomNumber);

    Task<List<Room>> GetRoomsByTypeRawAsync(string roomType);
}