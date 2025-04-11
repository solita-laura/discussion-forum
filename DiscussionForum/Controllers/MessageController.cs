using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
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
        [Route("/{id}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetAllMessages([FromQuery] [Range(0,Double.MaxValue)] int id)
        {
            try
            {
                var messages = await _context.Messages.Where(m => m.topicid == id).ToListAsync();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error fetching messages: {ex.Message}");
            }
        }

    }
}
