using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace BlazorCrudUsuarios.Application.Services;

public class TokenService
{
    
    private readonly IJSRuntime _js;

    public TokenService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<string?> GetToken()
    {
        return await _js.InvokeAsync<string>("localStorageHelper.get", "token");
    }
    
    
    public async Task<string?> GetRole()
    {
        var token = await GetToken();

        if (string.IsNullOrEmpty(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return jwt.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
    }


}