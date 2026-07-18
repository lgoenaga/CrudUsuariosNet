using ApiCrudUsuarios.Application.Dtos.Request;
using ApiCrudUsuarios.Application.Dtos.Response;

namespace ApiCrudUsuarios.Application.Interfaces;

public interface IUserService
{
    void CreateUser(RegisterRequest request);
    List<UserResponse> GetAllUsers();
    UserResponse GetMyUser(int userId);
    void DeleteUser(int id);
    void UpdateUser(int id, UpdateUserRequest request, string currentUserRole);
}