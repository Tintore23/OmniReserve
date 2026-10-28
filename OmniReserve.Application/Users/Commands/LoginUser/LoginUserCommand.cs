using MediatR;

namespace OmniReserve.Application.Users.Commands.LoginUser;

public record LoginUserCommand(
    string Email,
    string Password
) : IRequest<string>; // Devuelve un string que representa el token JWT