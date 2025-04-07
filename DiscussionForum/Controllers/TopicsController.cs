using System.Net;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Controllers
{
    
    [ApiController]
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
                return await _context.topics.ToListAsync();
            } catch {
                throw new Exception("Error fetching the topics.");
            }
        }
        
    }
}
