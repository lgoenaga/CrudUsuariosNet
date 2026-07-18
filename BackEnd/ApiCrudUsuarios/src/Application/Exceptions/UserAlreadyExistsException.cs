namespace ApiCrudUsuarios.Application.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() 
        : base("El usuario ya existe")
    {
    }

}