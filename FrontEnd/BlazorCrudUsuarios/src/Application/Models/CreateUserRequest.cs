namespace BlazorCrudUsuarios.Application.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global
public class CreateUserRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}