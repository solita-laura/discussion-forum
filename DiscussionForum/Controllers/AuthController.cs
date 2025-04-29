using DiscussionForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiscussionForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController (UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration){
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        protected int GetUserId ()
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value ?? "0");
        }

        //username: user1 password: test12
        /// <summary>
        /// Controller to handle the login request
        /// </summary>
        /// <param name="loginRequest">LoginRequest</param>
        /// <returns>string</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("login-user")]
        public async Task<ActionResult<string>> LoginUser([FromBody]LoginRequest loginRequest)
        {

            try{
                var user = await _userManager.FindByNameAsync(loginRequest.Username.ToUpper());

                var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    throw new InvalidPasswordException("Invalid user credentials.");
                }

                var token = GenerateJwtToken(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, //set to true if https is enabled (production etc.)
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                Response.Cookies.Append("token", token, cookieOptions);
                
                return Ok();

            }catch (UserNotFoundException ex){
                return NotFound(ex.Message);  
            }
            catch (InvalidPasswordException ex){
                return Unauthorized(ex.Message);
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
                return BadRequest("Login request failed.");
            }
        }

        [HttpPost("register-user")]
        public async Task<ActionResult<string>> RegisterUser([FromBody]RegistrationRequest registrationRequest)
        {
            try
            {
                var user = new User
                {
                    UserName = registrationRequest.Username,
                };

                var result = await _userManager.CreateAsync(user, registrationRequest.Password);

                if (!result.Succeeded)
                {
                    return BadRequest("User registration failed.");
                }

                return Ok("User registered successfully.");
            }
            catch
            {
                return BadRequest("Error registering user");
            }
        }

        /// <summary>
        /// Return the user id of the logged in user
        /// </summary>
        /// <returns>int</returns>

        [HttpGet]
        public ActionResult<int> GetUserInfo()
        {
            try
            {
                var userId = GetUserId();
                return Ok(userId);
            }
            catch
            {
                return BadRequest("Error fetching user information");
            }
        }

        /// <summary>
    /// Generate a jwt token once user is authenticated
    /// </summary>
    /// <param name="username">string</param>
    /// <returns>string</returns>
    private string GenerateJwtToken(User user)
    {
        try{
            var claims = new []
                {
                    new Claim("username", user.UserName),
                    new Claim("userid", user.Id.ToString()),
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
        }catch{
            throw new Exception("Error creating the JWT token");
        }
    }


    }
}
