namespace OmniReserve.Domain.ValueObjects;

public record EmailAddress
{
    public string Value { get; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
        {
            throw new ArgumentException("El formato del correo es inválido", nameof(value));
        }

        Value = value;
    }

    public override string ToString() => Value;
}