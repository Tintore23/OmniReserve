using Microsoft.EntityFrameworkCore;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;
using OmniReserve.Infrastructure.Persistence;

namespace OmniReserve.Infrastructure.Persistence.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ApplicationDbContext _context;

    public RoomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Room>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Rooms.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Room?> SearchByNumberAsync(string roomNumber)
    {
        return await _context.Rooms
            .FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
    }

    public async Task<List<Room>> GetRoomsByTypeRawAsync(string roomType)
    {
        return await _context.Rooms
            .FromSql($"SELECT * FROM \"Rooms\" WHERE \"Type\" = {roomType}")
            .ToListAsync();
    }
}