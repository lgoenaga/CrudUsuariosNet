using ApiCrudUsuarios.Application.Dtos.Request;
using ApiCrudUsuarios.Application.Dtos.Response;

namespace ApiCrudUsuarios.Application.Interfaces;

public interface IAuthService
{
    void Register(RegisterRequest request);
    LoginResponse Login(LoginRequest request);
}