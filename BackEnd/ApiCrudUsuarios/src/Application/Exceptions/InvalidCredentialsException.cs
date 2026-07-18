namespace ApiCrudUsuarios.Application.Exceptions;

public class InvalidCredentialsException : Exception
{
    
    public InvalidCredentialsException() 
        : base("Contraseña incorrecta")
    {
    }


}