namespace ApiCrudUsuarios.Application.Dtos.Request;

public class UpdateUserRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; } // 🔥 Admin only

}