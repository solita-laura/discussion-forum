using DiscussionForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController (UserManager<User> userManager, SignInManager<User> signInManager){
            _userManager = userManager;
            _signInManager = signInManager;
        }

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
                var user = await _userManager.FindByNameAsync(loginRequest.Username);

                var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest("Login request failed.");

            }catch{
                return BadRequest("Login request failed.");
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="registrationRequest">RegistrationRequest</param>
        /// <returns>string</returns>

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

                if(result.ToString().Contains("DuplicateUserName")){
                    return Conflict("Username is already in use");
                }

                if (result.Succeeded){
                    await _userManager.AddToRoleAsync(user, "User");
                    return Ok("User registered successfully.");
                }

                return BadRequest("Error registering user");
            }
            catch
            {
                return BadRequest("Error registering user");
            }
        }

        /// <summary>
        /// log out the current user
        /// </summary>
        /// <returns>string</returns>

        [HttpPost("logout-user")]
        [Authorize]
        public async Task<ActionResult<string>> LogoutUser ()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok("User signed out succusefully");
            }
            catch
            {
                return BadRequest("Error in sign out.");
            }
        }



        /// <summary>
        /// Return the user id of the logged in user
        /// </summary>
        /// <returns>string</returns>

        [HttpGet("get-userid")]
        [Authorize]
        public ActionResult<string> GetUserInfo()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if(userId==null){
                    return BadRequest("User not logged in");
                }
                return Ok(userId);
            }
            catch
            {
                return BadRequest("Error fetching user information");
            }
        }

        /// <summary>
        /// Return the users role
        /// </summary>
        /// <returns>string</returns>

        [HttpGet("get-userrole")]
        [Authorize]
        public async Task<ActionResult<string>> GetUserRole()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var userRole = await _userManager.GetRolesAsync(user);
                return Ok(userRole);
            }
            catch
            {
                return BadRequest("Error fetching user role");
            }
        }


    }
}
