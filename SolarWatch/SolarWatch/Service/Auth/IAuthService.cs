namespace SolarWatch.Service.Auth;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string username, string password, string role);
    
    Task<AuthResult> LoginAsync(string username, string password);
}