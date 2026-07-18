using System.Net.Http.Json;
using BlazorCrudUsuarios.Application.Interfaces;
using BlazorCrudUsuarios.Application.Models;
using BlazorCrudUsuarios.Shared.Constants;

namespace BlazorCrudUsuarios.Application.Services;

public class UserService : IUserService
{
    private readonly IAuthService _authService;
    private readonly HttpClient _httpClient;
    
    public UserService(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }
    
    
    public async Task<List<UserResponse>> GetUsers()
    {
        await _authService.SetAuthHeader();
        return await _httpClient.GetFromJsonAsync<List<UserResponse>>(ApiRoutes.Users.Base)
               ?? new List<UserResponse>();
    }
    
    
    public async Task CreateUser(CreateUserRequest request)
    {
        await _authService.SetAuthHeader();
        var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Users.Base, request);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Error creando usuario");
    }
    
    public async Task DeleteUser(int id)
    {
        await _authService.SetAuthHeader();
        var response = await _httpClient.DeleteAsync(ApiRoutes.Users.ById(id));
        if (!response.IsSuccessStatusCode)
            throw new Exception("Error eliminando usuario");
    }
    
    public async Task UpdateUser(int id, CreateUserRequest request)
    {
        await _authService.SetAuthHeader();
        var response = await _httpClient.PutAsJsonAsync(ApiRoutes.Users.ById(id), request);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Error actualizando usuario");
    }

}