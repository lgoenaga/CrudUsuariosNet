using ApiCrudUsuarios.Application.Dtos.Request;
using ApiCrudUsuarios.Application.Interfaces;
using ApiCrudUsuarios.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCrudUsuarios.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
 
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
 
    
    [HttpGet("admin")]
    [Authorize(Roles = Roles.Admin)]
    public IActionResult OnlyAdmin()
    {
        return Ok("Solo acceso para Admin 🔒");
    }

    
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        
        // ✅ Validaciones básicas
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("Email es requerido");

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Password es requerido");

        if (request.Password.Length < 6)
            return BadRequest("Password debe tener mínimo 6 caracteres");

        if (!request.Email.Contains("@"))
            return BadRequest("Email inválido");
        
        _authService.Register(request);
        
        return Ok("Usuario registrado exitosamente");
    }

    
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        
        // ✅ Validaciones básicas
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("Email es requerido");

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Password es requerido");
        
        var result = _authService.Login(request);

        return Ok(result);
    }
    
    
    [HttpGet("profile")]
    [Authorize]
    public IActionResult Profile()
    {
        return Ok("Usuario autenticado ✅");
    }


}