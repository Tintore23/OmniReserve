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
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public EmailAddress Email { get; private set; } 
    public string PasswordHash { get; private set; } // <-- Cambiado de Password a PasswordHash
    public Role UserRole { get; private set; }
    public ICollection<Reservation> Reservations { get; private set; } = new List<Reservation>();

    public User(string firstName, string lastName, EmailAddress email, string passwordHash, Role userRole)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name required");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name required");
        if (email is null) throw new ArgumentNullException(nameof(email), "Email required"); 
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password hash required");

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash; // <-- Asignando el hash seguro
        UserRole = userRole;
    }
}