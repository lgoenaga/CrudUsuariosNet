using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace BlazorCrudUsuarios.Application.Services;


public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly TokenService _tokenService;

    public CustomAuthenticationStateProvider(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenService.GetToken();

        var identity = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // 🔥 obtener email para mostrar en navbar
            var emailClaim = jwt.Claims
                .FirstOrDefault(c => c.Type.Contains("email"));

            if (emailClaim != null)
            {
                // ✅ crear identidad con Name correcto + todos los claims
                identity = new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.Name, emailClaim.Value) }
                        .Concat(jwt.Claims),
                    "jwt"
                );
            }
            else
            {
                // fallback (por si no hay email)
                identity = new ClaimsIdentity(jwt.Claims, "jwt");
            }
        }

        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }
 
    public void NotifyUserAuthentication(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var emailClaim = jwt.Claims.FirstOrDefault(c => c.Type.Contains("email"));

        ClaimsIdentity identity;

        if (emailClaim != null)
        {
            identity = new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Name, emailClaim.Value) }
                    .Concat(jwt.Claims),
                "jwt"
            );
        }
        else
        {
            identity = new ClaimsIdentity(jwt.Claims, "jwt");
        }

        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(user))
        );
    }

}