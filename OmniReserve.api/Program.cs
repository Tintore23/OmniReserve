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

app.UseMiddleware<OmniReserve.Api.Middlewares.GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // 1. Lee, inspecciona y valida el JWT Bearer
app.UseAuthorization();  // 2. Valida los permisos/roles del usuario autenticado

app.MapControllers();

app.Run();