using DiscussionForum.Models;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                return response;
            }catch{
                throw new Exception("Error while logging in the user.");
            }
        }

        
    }
}
