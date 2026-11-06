using OmniReserve.Domain.Constants; // Asegúrate de importar el namespace de Roles
using OmniReserve.Domain.ValueObjects;

namespace OmniReserve.Domain.Entities;

public class User
{
    public enum Role
    {
        Admin,
        Customer
    }

    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty; // Soluciona el warning CS8618
    public string LastName { get; private set; } = string.Empty;  // Soluciona el warning CS8618
    public EmailAddress Email { get; private set; } = null!;      // Soluciona el warning CS8618
    public string PasswordHash { get; private set; } = string.Empty; // Soluciona el warning CS8618
    public Role UserRole { get; private set; }
    public ICollection<Reservation> Reservations { get; private set; } = new List<Reservation>();

    // Constructor principal
    public User(string firstName, string lastName, EmailAddress email, string passwordHash, Role userRole)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name required");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name required");
        if (email is null) throw new ArgumentNullException(nameof(email), "Email required"); 
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password hash required");

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName; // <-- Corregido: asigna el parámetro en minúscula
        Email = email;
        PasswordHash = passwordHash;
        UserRole = userRole;
    }

    // Constructor protegido para EF Core
    protected User() { } 
}
