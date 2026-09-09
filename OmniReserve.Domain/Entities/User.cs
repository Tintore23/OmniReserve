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
    public string Email { get; private set; }
    public string Password { get; private set; }
    public Role UserRole { get; private set; }

    public User(string firstName, string lastName, string email, string password, Role userRole)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name required");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name required");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email required");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password required");

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        UserRole = userRole;
    }
}