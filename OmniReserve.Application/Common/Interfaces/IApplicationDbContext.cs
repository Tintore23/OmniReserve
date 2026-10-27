using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Room> Rooms { get; }
    DbSet<User> Users { get; }
    DbSet<Reservation> Reservations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}