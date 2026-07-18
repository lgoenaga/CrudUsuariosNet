using ApiCrudUsuarios.Application.Dtos.Request;
using ApiCrudUsuarios.Application.Dtos.Response;
using ApiCrudUsuarios.Application.Exceptions;
using ApiCrudUsuarios.Application.Interfaces;
using ApiCrudUsuarios.Domain.Constants;
using ApiCrudUsuarios.Domain.Models;
using ApiCrudUsuarios.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace ApiCrudUsuarios.Application.Services;

public class UserService : IUserService
{
    
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    
    public UserService(AppDbContext context,
        IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }


    public void CreateUser(RegisterRequest request)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        
        // ✅ Validación de negocio
        if (_context.Users.Any(u => u.Email == normalizedEmail))
            throw new UserAlreadyExistsException();

        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = "",
            Role = Roles.User // ✅ por defecto
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        user.Validate();
        
        _context.Users.Add(user);
        _context.SaveChanges();

    }
    
    
    public List<UserResponse> GetAllUsers()
    {
        
        return _context.Users
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Email = u.Email,
                Role = u.Role
            })
            .ToList();

    }
    
    public UserResponse GetMyUser(int userId)
    {
  
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
            throw new UserNotFoundException();

        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role
        };

    }
    
    public void DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            throw new UserNotFoundException();

        _context.Users.Remove(user);
        _context.SaveChanges();
    }
    
    public void UpdateUser(int id, UpdateUserRequest request, string currentUserRole)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
            throw new UserNotFoundException();

        // ✅ Email
   
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var newEmail = request.Email.Trim().ToLowerInvariant();
            var currentEmail = user.Email.ToLowerInvariant();

            if (newEmail != currentEmail)
            {
                var emailExists = _context.Users
                    .Any(u => u.Email == newEmail);

                if (emailExists)
                    throw new Exception("El email ya está en uso");

                user.Email = newEmail;
            }
        }
        
        // ✅ Password
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        }

        // 🔥 Role → solo Admin
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (currentUserRole != Roles.Admin)
                throw new Exception("No tienes permisos para cambiar el rol");

            user.Role = request.Role;
        }

        _context.SaveChanges();
    }
    
}