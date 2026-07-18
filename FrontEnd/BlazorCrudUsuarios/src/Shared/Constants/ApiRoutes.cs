namespace BlazorCrudUsuarios.Shared.Constants;

public static class ApiRoutes
{
    public static class Auth
    {
        public const string Login = "api/auth/login";
    }

    public static class Users
    {
        public const string Base = "api/users";
        public static string ById(int id) => $"api/users/{id}";
        public const string Me = "api/users/me";
    }
}

