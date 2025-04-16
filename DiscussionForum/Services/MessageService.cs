using System;
using DiscussionForum.DbEntities;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult> CreateMessage(Message message)
        {
            try
            {
                _msgContext.messages.Add(message);
                var response = await _msgContext.SaveChangesAsync();

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

}
