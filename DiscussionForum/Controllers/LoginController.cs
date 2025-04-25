using DiscussionForum.Models;
using DiscussionForum.Services;
using Humanizer.Localisation.DateToOrdinalWords;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DiscussionForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserService _userService;

        public LoginController (UserService userService){
            _userService = userService;
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
        [HttpPost]
        public async Task<ActionResult<string>> LoginUser([FromBody]LoginRequest loginRequest)
        {
            try{
                var response = await _userService.Login(loginRequest);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, //set to true if https is enabled (production etc.)
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                Response.Cookies.Append("token", response.Token, cookieOptions);
                
                return Ok();
            }catch (UserNotFoundException ex){
                return NotFound(ex.Message);  
            }
            catch (InvalidPasswordException ex){
                return Unauthorized(ex.Message);
            }
            catch (Exception ex){
                return BadRequest("Login request failed.");
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


    }
}
