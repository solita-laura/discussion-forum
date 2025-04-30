using System.Net;
using DiscussionForum.Models;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Controllers
{
    
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly TopicContext _context;
        private readonly MessageService _messageService;
        private readonly UserManager<User> _userManager;

        public TopicsController (TopicContext context, MessageService messageService, UserManager<User> userManager){
            _context = context;
            _messageService = messageService;
            _userManager=userManager;
        }

        /// <summary>
        /// Get all topics
        /// </summary>
        /// <returns>List of topics</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetAllTopics()
        {
            try{
                return await _context.topics.OrderByDescending(t => t.lastupdated).ToListAsync();
            } catch {
                return BadRequest("Error fetching the topics.");
            }
        }

        /// <summary>
        /// post a new topic to the database.
        /// Required attribute is content
        /// </summary>
        /// <param name="topic">Topic</param>
        /// <returns>status code</returns>

        [HttpPost]
        public async Task<ActionResult> CreateTopic([FromBody] Topic topic)
        {
            try
            {
                var id = _userManager.GetUserId(User);
                Console.WriteLine(id);
                topic.userid = id;
                _context.topics.Add(topic);
                await _context.SaveChangesAsync();
                return Ok("Topic created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error creating topic");
            }
        }

        /// <summary>
        /// update the name of the topic if message count is 0
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="topicname">string</param>
        /// <returns>status code</returns>

        [HttpPut]
        public async Task<ActionResult> UpdateTopic([FromQuery(Name ="topicid")] int id, [FromBody] string topicname)
        {

            try
            {
                var topic = await _context.topics.FindAsync(id);

                //topic name can only be updated if the message count is 0
                if (topic.messagecount != 0)
                {
                    return BadRequest("Topic name cannot be updated as it has messages.");
                }

                var userid =_userManager.GetUserId(User);
                if (topic.userid != userid)
                {
                    return BadRequest("You are not authorized to update this topic.");
                }

                topic.topicname = topicname;
                await _context.SaveChangesAsync();

                return Ok("Topic updated successfully");
            }
            catch
            {
                return BadRequest("Error updating topic.");
            }
        }

        /// <summary>
        /// delete the topic
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>status code</returns>

        [HttpDelete]
        public async Task<ActionResult> DeleteTopic([FromQuery(Name = "topicid")] int id)
        {
            try
            {
                var topic = await _context.topics.FindAsync(id);

                var userid =_userManager.GetUserId(User);
                if (topic.userid != userid)
                {
                    return BadRequest("You are not authorized to delete this topic.");
                }

                //delete the messages first
                if (topic.messagecount != 0)
                {
                    await _messageService.DeleteMessage(id);
                }

                _context.topics.Remove(topic);
                await _context.SaveChangesAsync();

                return Ok("Topic deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest("Error deleting topic");
            }
        }
        
    }
}
