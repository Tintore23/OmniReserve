using System.Threading.Tasks;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
}