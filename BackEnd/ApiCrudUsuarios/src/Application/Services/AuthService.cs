using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiCrudUsuarios.Application.Dtos.Request;
using ApiCrudUsuarios.Application.Dtos.Response;
using ApiCrudUsuarios.Application.Exceptions;
using ApiCrudUsuarios.Application.Interfaces;
using ApiCrudUsuarios.Domain.Constants;
using ApiCrudUsuarios.Domain.Models;
using ApiCrudUsuarios.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ApiCrudUsuarios.Application.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(
        AppDbContext context,
        IPasswordHasher<User> passwordHasher,
        IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public void Register(RegisterRequest request)
    {
        if (_context.Users.Any(u => u.Email == request.Email))
            throw new UserAlreadyExistsException();

        var user = new User
        {
            Email = request.Email,
            PasswordHash = "",
            Role = Roles.User
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        
        user.Validate();

        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public LoginResponse Login(LoginRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user == null)
            throw new UserNotFoundException();

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (result == PasswordVerificationResult.Failed)
            throw new InvalidCredentialsException();

        var keyString = _configuration["JWT:KEY"];

        if (string.IsNullOrEmpty(keyString))
            throw new Exception("JWT:KEY no configurado");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(keyString)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponse
        {
            Token = jwt
        };
    }
}