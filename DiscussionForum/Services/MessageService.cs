using System;
using DiscussionForum.DbEntities;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Services;

public class MessageService
{
    private readonly MessageContext _msgContext;
    private readonly TopicContext _topicContext;

    public MessageService(MessageContext msgContext, TopicContext topicContext)
    {
        _msgContext = msgContext;
        _topicContext = topicContext;
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
                _msgContext.messages.Add(message);
                var response = await _msgContext.SaveChangesAsync();

                //TODO: if this fails delete the message and send badrequest
                //if the message is saved succesfully, update the message count and update time for the corresponding topic
                //calculate message count from the messages table
                if(response>0){
                    var topic = _topicContext.topics.FirstOrDefault(t => t.id.Equals(message.topicid));
                    topic.messagecount += 1;
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
