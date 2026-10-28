using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.ValueObjects;

namespace OmniReserve.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
       
        string secureHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            request.FirstName,
            request.LastName,
            new EmailAddress(request.Email), // Ajusta si tu EmailAddress se instancia diferente
            secureHash,
            request.UserRole
        );

      
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}