using BlazorCrudUsuarios.Application.Models;

namespace BlazorCrudUsuarios.Application.Interfaces;

public interface IUserService
{
    Task<List<UserResponse>> GetUsers();
    Task CreateUser(CreateUserRequest request);
    Task DeleteUser(int id);
    Task UpdateUser(int id, CreateUserRequest request);
}