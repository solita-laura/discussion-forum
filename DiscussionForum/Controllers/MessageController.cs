using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DiscussionForum.DbEntities;
using DiscussionForum.Services;
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
        private readonly MessageService _messageService;
        public MessageController(MessageContext context, MessageService messageService)
        {
            _messageService = messageService;
            _context = context;
        }

        protected int GetUserId ()
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value ?? "0");
        }

        /// <summary>
        /// Get all messages from database with the specific topic id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Message</returns>

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

        /// <summary>
        /// post a message for the specific topic
        /// required attributes for message are content, topic id and user id
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>status code</returns>

        [HttpPost]
        public async Task<ActionResult> CreateMessage([FromBody] Message message)
        {
            try
            {
                var id = GetUserId();
                message.userid = id;
                await _messageService.CreateMessage(message);
                return Ok("message created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error creating message");
            }
        }

        /// <summary>
        /// update the message if the user has created it themselves
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Status code</returns>

        [HttpPut]
        public async Task<ActionResult> UpdateMessage([FromQuery(Name = "messageid")] int messageid, [FromBody] string messagecontent)
        {
            try
            {
                var id = GetUserId();
                var msg = await _context.messages.FirstOrDefaultAsync(m => m.id == messageid);

                if(msg.userid != id)
                {
                    return Unauthorized("You are not authorized to update this message");
                }

                msg.content = messagecontent;
                await _context.SaveChangesAsync();
                return Ok("message updated successfully");
            }
            catch
            {
                return BadRequest("Error updating message");
            }
        }

    }
}
