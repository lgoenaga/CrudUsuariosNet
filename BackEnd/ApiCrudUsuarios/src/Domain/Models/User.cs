using System.ComponentModel.DataAnnotations;

namespace ApiCrudUsuarios.Domain.Models;

public class User
{
    public int Id { get; private set; }
    
    [MaxLength(256)]
    public required string Email { get; set; }
    
    [MaxLength(512)]
    public required string PasswordHash { get; set; }
    
    [MaxLength(50)]
    public required string Role { get; set; }
    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Email))
            throw new Exception("Email inválido");

        if (string.IsNullOrWhiteSpace(PasswordHash))
            throw new Exception("PasswordHash inválido");
    }
    
}