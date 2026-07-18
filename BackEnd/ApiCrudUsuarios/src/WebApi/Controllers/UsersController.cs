using System.Security.Claims;
using ApiCrudUsuarios.Application.Dtos.Request;
using ApiCrudUsuarios.Application.Interfaces;
using ApiCrudUsuarios.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCrudUsuarios.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
   
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public IActionResult CreateUser(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("Email es requerido");

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Password es requerido");

        if (request.Password.Length < 6)
            return BadRequest("Password mínimo 6 caracteres");

        _userService.CreateUser(request);
        
        return Ok("Usuario creado por Admin ✅");
    }
    
    
    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();

        return Ok(users);
    }
    
    
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetMyUser()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var user = _userService.GetMyUser(userId);

        return Ok(user);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public IActionResult DeleteUser(int id)
    {
        // ✅ Obtener el usuario autenticado
        var currentUserId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        // ✅ Validar que no se elimine a sí mismo
        if (currentUserId == id)
        {
            return BadRequest("No puedes eliminarte a ti mismo");
        }
        
        _userService.DeleteUser(id);
        
        return Ok("Usuario eliminado ✅");
    }
    
    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateUser(int id, UpdateUserRequest request)
    {
        var currentUserId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value
        );

        var currentUserRole =
            User.FindFirst(ClaimTypes.Role)!.Value;

        // ✅ User solo puede editarse a sí mismo
        if (currentUserRole != Roles.Admin && currentUserId != id)
            return Forbid();

        _userService.UpdateUser(id, request, currentUserRole);

        return Ok("Usuario actualizado ✅");
    }
    
}