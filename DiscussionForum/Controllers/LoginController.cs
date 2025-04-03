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

        [HttpPost]
        public async Task<ActionResult<string>> LoginUser([FromBody] LoginRequest loginRequest)
        {
            
            var response = await _userService.Login(loginRequest);
            return response;
        }

        
    }
}
