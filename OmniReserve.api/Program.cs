using OmniReserve.Application;
using OmniReserve.Infrastructure;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Prueba de Reglas de Excepcion de dominio me ayudo gemini para saber si copilaba corectamente 
try
{
    // Provocamos la excepción mandando una fecha de CheckOut anterior al CheckIn
    var reservaInvalida = new Reservation(
        Guid.NewGuid(),
        Guid.NewGuid(),
        DateTime.Now.AddDays(2),
        DateTime.Now,
        100
    );
}
catch (DomainException ex)
{
    Console.WriteLine($"\n[PRUEBA DOMINIO - ÉXITO] Excepción capturada: {ex.GetType().Name}");
    Console.WriteLine($"Mensaje de error: {ex.Message}\n");
}
// -----------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();