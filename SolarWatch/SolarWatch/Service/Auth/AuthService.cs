﻿using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Service.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager; //CreateAsync a UserManager osztályban, valójában adatbázis műveleteket hajtanak végre.

    private readonly ITokenService _tokenService;
    
    public AuthService(UserManager<IdentityUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role)
    {
        var user = new IdentityUser { UserName = username, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return FailedRegistration(result, email, username);
        }

        await _userManager.AddToRoleAsync(user, role); // Adding the user to a role
        return new AuthResult(true, email, username, "");
    }

    private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
    {
        var authResult = new AuthResult(false, email, username, "");

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    }
    
    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);

        if (managedUser == null)
        {
            return InvalidEmail(email);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid)
        {
            return InvalidPassword(email, managedUser.UserName);
        }

        var roles = await _userManager.GetRolesAsync(managedUser);
        
       
       if (roles.Count == 0)
        {
            await _userManager.AddToRoleAsync(managedUser, "user"); // Adding the user to a role
            roles = await _userManager.GetRolesAsync(managedUser);
        }
        
        var accessToken  = _tokenService.CreateToken(managedUser, roles[0]);
        

        return new AuthResult(true, managedUser.Email, managedUser.UserName, accessToken);
    }

    private static AuthResult InvalidEmail(string email)
    {
        var result = new AuthResult(false, email, "", "");
        result.ErrorMessages.Add("Bad credentials", "Invalid email");
        return result;
    }

    private static AuthResult InvalidPassword(string email, string userName)
    {
        var result = new AuthResult(false, email, userName, "");
        result.ErrorMessages.Add("Bad credentials", "Invalid password");
        return result;
    }
}