using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DiscussionForum.Services;


public class UserService : IAuthenticationService
{
    private readonly UserContext _context;
    private readonly IConfiguration _configuration;

    public UserService (IConfiguration configuration, UserContext context){
        _configuration=configuration;
        _context = context;
    }

    public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
    {
        throw new NotImplementedException();
    }

    public async Task<string> Login (LoginRequest loginRequest){

        try{
            
            var user = _context.users.FirstOrDefault(u => u.username.Equals(loginRequest.Username));

            if(user is null){
                Console.WriteLine("no user found");
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.password,
                salt: user.salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256/8
            ));

            if(user.password.Equals(hashed))
            {
                var token = GenerateJwtToken(user.username);
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
