using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using DiscussionForum.DbEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Controllers
{
    [Route("api/Topics/[controller]")]
    [ApiController]
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
                var messages = await _context.messages.Where(m => m.topicid == id).ToListAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error fetching messages");
            }
        }

    }
}
