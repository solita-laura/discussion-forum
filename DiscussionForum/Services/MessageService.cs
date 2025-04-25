using DiscussionForum.DbEntities;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services;

public class MessageService
{
    private readonly MessageContext _msgContext;
    private readonly TopicContext _topicContext;
    private readonly UserContext _userContext;

    public MessageService(MessageContext msgContext, TopicContext topicContext, UserContext userContext)
    {
        _msgContext = msgContext;
        _topicContext = topicContext;
        _userContext = userContext;
    }

    /// <summary>
    /// Get all messages for a topic and return them as a list of MessageResponse objects.
    /// </summary>
    /// <param name="topicid">int</param>
    /// <returns>List<MessageResponse></returns>

    public async Task<List<MessageResponse>> GetAllMessages(int topicid)
    {
        try
        {
            var messages = await _msgContext.messages.Where(m => m.topicid == topicid).ToListAsync();
            var messageResponses = messages.Select(m => new MessageResponse
            {
                id = m.id,
                content = m.content,
                topicid = m.topicid,
                userid = m.userid,
                username = _userContext.users.FirstOrDefault(u => u.id == m.userid)?.username,
                upvotes = m.upvotes,
                postdate = m.postdate
            }).ToList();

            messageResponses = messageResponses.OrderBy(m => m.postdate).ToList();

            return messageResponses;
        }
        catch (Exception ex)
        {
            return new List<MessageResponse>();
        }
    }

    /// <summary>
    /// create a message to the database.
    /// A new message should also update the topic message count and updating time for the topic
    /// </summary>
    /// <param name="message">Message</param>
    /// <returns>status code</returns>

    public async Task<ActionResult> CreateMessage(Message message)
        {
            try
            {
                message.postdate = DateTime.UtcNow;
                _msgContext.messages.Add(message);
                var response = await _msgContext.SaveChangesAsync();

                if(response>0){
                    var topic = _topicContext.topics.FirstOrDefault(t => t.id.Equals(message.topicid));
                    topic.messagecount = _msgContext.messages.Count(m => m.topicid == message.topicid);
                    topic.lastupdated = message.postdate;
                    await _topicContext.SaveChangesAsync();
                }
                return new OkObjectResult("Message created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult("Error creating message");
            }
        }

    /// <summary>
    /// delete all messages for a topic
    /// </summary>
    /// <param name="topicid">int</param>
    /// <returns>status code</returns>

    public async Task<ActionResult> DeleteMessage(int topicid)
    {
        try
        {
            var messages = _msgContext.messages.Where(m => m.topicid == topicid).ToList();
            _msgContext.messages.RemoveRange(messages);
            var response = await _msgContext.SaveChangesAsync();

            return new OkObjectResult("Messages deleted successfully");

        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new BadRequestObjectResult("Error deleting message");
        }
    }

}
