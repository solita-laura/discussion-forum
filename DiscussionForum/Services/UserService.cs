using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DiscussionForum.Services;


public class UserService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public UserService (UserManager<User> userManager, IConfiguration configuration){
        _userManager=userManager;
        _configuration=configuration;
    }

    public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Login (LoginRequest loginRequest){

        try{
            
            Console.WriteLine(loginRequest.Email);
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if(user is null){
                Console.WriteLine("no user found");
            }

            if(await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                var token = GenerateJwtToken(user.UserName);
                return token;
            }

            throw new Exception("No match in passwords."); 

        }catch (Exception ex){
            Console.WriteLine(ex.Message);
            throw new Exception("Login request failed."); 
        }
    }

    public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    private string GenerateJwtToken(string username)
    {
        var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:ValidIssuer"],
            audience: _configuration["Jwt:ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
