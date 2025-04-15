using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DiscussionForum.DbEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Controllers
{
    [Route("api/Topics/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly MessageContext _context;
        public MessageController(MessageContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetAllMessages([FromQuery (Name ="topicid")] [Range(0,int.MaxValue)] int id)
        {
            try
            {
                var messages = await _context.messages.Where(m => m.topicid == id).OrderBy(m => m.postdate).ToListAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error fetching messages");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage([FromBody] Message message)
        {
            try
            {
                _context.messages.Add(message);
                await _context.SaveChangesAsync();
                return Ok("message created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error creating message");
            }
        }

    }
}
