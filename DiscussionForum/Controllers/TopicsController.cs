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
        
    }
}
