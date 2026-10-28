using MediatR;
using OmniReserve.Domain.Entities; 

namespace OmniReserve.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password, 
    User.Role UserRole
) : IRequest<Guid>;