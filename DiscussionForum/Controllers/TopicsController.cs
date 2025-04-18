using System.Net;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Controllers
{
    
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly TopicContext _context;

        public TopicsController (TopicContext context){
            _context = context;
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

        [HttpPut]
        public async Task<ActionResult> UpdateTopic([FromQuery(Name ="topicid")] int id, [FromBody] string topicname)
        {

            try
            {
                var topic = await _context.topics.FindAsync(id);
                if (topic.messagecount != 0)
                {
                    return BadRequest("Topic name cannot be updated as it has messages.");
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
        
    }
}
