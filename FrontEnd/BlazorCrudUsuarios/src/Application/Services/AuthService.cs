using System.Net.Http.Headers;
using System.Net.Http.Json;
using BlazorCrudUsuarios.Application.Interfaces;
using BlazorCrudUsuarios.Application.Models;
using BlazorCrudUsuarios.Shared.Constants;

namespace BlazorCrudUsuarios.Application.Services;

public class AuthService : IAuthService
{
    
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;

    public AuthService(HttpClient httpClient, TokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }
    
    public async Task SetAuthHeader()
    {
        var token = await _tokenService.GetToken();

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
    
    public async Task<string> Login(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Auth.Login, request);
        
        if (!response.IsSuccessStatusCode)
            throw new Exception("Error en login");

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        return result!.Token;

    }
}