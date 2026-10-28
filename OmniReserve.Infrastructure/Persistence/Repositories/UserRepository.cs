using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;
using OmniReserve.Infrastructure.Persistence;

namespace OmniReserve.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        // Nota: Como 'Email' es un ValueObject, evaluamos su propiedad .Value o comparamos directamente según tu diseño
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.Value == email); 
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }
}