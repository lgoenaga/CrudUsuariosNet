namespace ApiCrudUsuarios.Application.Dtos.Response;

public class UserResponse
{
    
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

}