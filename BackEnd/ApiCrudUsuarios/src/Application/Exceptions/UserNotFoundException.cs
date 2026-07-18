namespace ApiCrudUsuarios.Application.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() 
        : base("Usuario no existe")
    {
    }
}