using DiscussionForum.DbEntities;
using DiscussionForum.Models;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;

public class MessageService_UnitTests {

    [Test]
    public async Task CreateMessage_Success()
    {
        var msgContextMock = new Mock<MessageContext>();
        var topicContextMock = new Mock<TopicContext>();

        var topicData = new List<Topic>
        {
            new Topic{ id=1, topicname="Test topic", messagecount=0, lastupdated=DateTime.UtcNow.AddDays(-1)}
        };

        topicContextMock.Setup(t => t.topics)
                        .ReturnsDbSet(topicData);
        
        topicContextMock.Setup(t => t.SaveChangesAsync(default))
                    .ReturnsAsync(1);

        var msgData = new List<Message>();

        msgContextMock.Setup(m => m.messages)
                        .ReturnsDbSet(msgData);
        
        msgContextMock.Setup(m => m.SaveChangesAsync(default))
                  .ReturnsAsync(1);

        var msgService = new MessageService(msgContextMock.Object, topicContextMock.Object);

        var response = await msgService.CreateMessage(new Message{topicid=1, userid=1, content="some content"});
        Assert.That(response, Is.TypeOf(typeof(OkObjectResult)));

        msgContextMock.Verify(m => m.messages.Add(It.IsAny<Message>()), Times.Once);
        msgContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);

        topicContextMock.Verify(t=> t.SaveChangesAsync(default), Times.Once);

        var updatedTopic = topicData.FirstOrDefault(t => t.id==1);

        Assert.That(updatedTopic.messagecount, Is.EqualTo(1));
        Assert.That(updatedTopic.lastupdated, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromSeconds(1)));

    }

}