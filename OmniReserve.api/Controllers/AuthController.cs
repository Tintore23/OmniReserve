using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting; // 1. Importa el espacio de nombres

namespace OmniReserve.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // [HttpPost("login")] o la ruta exacta que utilices para el inicio de sesión
    [HttpPost("login")]
    [EnableRateLimiting("AuthLimit")] // 2. Aplica la política restrictiva de fuerza bruta
    public IActionResult Login([FromBody] object loginModel) // Reemplaza 'object' con tu DTO de login (ej. LoginRequestDto)
    {
        // Tu lógica actual de autenticación
        return Ok(new { message = "Intento de inicio de sesión recibido." });
    }
}