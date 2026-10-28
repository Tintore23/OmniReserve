using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniReserve.Application.Common.Interfaces;

namespace OmniReserve.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // Paso 1: Buscar al usuario por su Email
        var user = await _userRepository.GetByEmailAsync(request.Email);

        // Paso 2 y 4: Si el usuario no existe o la contraseña no coincide, arrojamos un error genérico (por seguridad)
        if (user is null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Paso 3: Verificar el Hash de la contraseña
        bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Paso 5: Si todo es válido, generar el token JWT y retornarlo
        var token = _jwtProvider.Generate(user);

        return token;
    }
}