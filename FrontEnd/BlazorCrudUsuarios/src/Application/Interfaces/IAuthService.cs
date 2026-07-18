using BlazorCrudUsuarios.Application.Models;

namespace BlazorCrudUsuarios.Application.Interfaces;

public interface IAuthService
{
    Task<string> Login(LoginRequest request);
    Task SetAuthHeader();
}