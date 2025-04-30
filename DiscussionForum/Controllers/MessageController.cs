using System.ComponentModel.DataAnnotations;
using DiscussionForum.DbEntities;
using DiscussionForum.Models;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        public MessageController(MessageContext context, MessageService messageService, UserManager<User> userManager)
        {
            _messageService = messageService;
            _context = context;
            _userManager=userManager;
        }

        /// <summary>
        /// Get all messages from database with the specific topic id
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Message</returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageResponse>>> GetAllMessages([FromQuery (Name ="topicid")] [Range(0,int.MaxValue)] int id)
        {
            try
            {
                var messages = await _messageService.GetAllMessages(id);
                return Ok(messages);
            }
            catch
            {
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
                var id = _userManager.GetUserId(User);
                message.userid = id;
                message.postdate = DateTime.UtcNow;

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
                var id = _userManager.GetUserId(User);
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
