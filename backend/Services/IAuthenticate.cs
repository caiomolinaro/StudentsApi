namespace StudentsApi.Services
{
    public interface IAuthenticate
    {
        Task<bool> RegisterUserAsync(string email, string password);

        Task<bool> AuthenticateUserAsync(string email, string password);

        Task LogoutAsync();
    }
}