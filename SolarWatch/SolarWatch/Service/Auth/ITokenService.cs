using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Service.Auth;

public interface ITokenService
{
    string CreateToken(IdentityUser user, string role);
}