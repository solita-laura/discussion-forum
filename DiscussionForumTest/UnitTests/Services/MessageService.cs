using DiscussionForum.DbEntities;
using DiscussionForum.Models;
using DiscussionForum.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;

public class MessageService_UnitTests {

    /// <summary>
    /// Test creating a message.
    /// The message should be created successfully.
    /// </summary>

    [Test]
    public async Task CreateMessage_Success()
    {
        var msgContextMock = new Mock<MessageContext>();
        var topicContextMock = new Mock<TopicContext>();
        var userContextMock = new Mock<UserContext>();

        var topicData = new List<Topic>
        {
            new Topic{ id=1, topicname="Test topic", messagecount=0, lastupdated=DateTime.UtcNow.AddDays(-1)}
        };

        topicContextMock.Setup(t => t.topics)
                        .ReturnsDbSet(topicData);
        
        topicContextMock.Setup(t => t.SaveChangesAsync(default))
                    .ReturnsAsync(1);

        var msgData = new List<Message>
        {
            new Message{ id=1, topicid=1, userid=1, content="some content", postdate=DateTime.UtcNow},
            new Message{ id=2, topicid=2, userid=2, content="some other content", postdate=DateTime.UtcNow}
        };


        msgContextMock.Setup(m => m.messages)
                        .ReturnsDbSet(msgData);
        
        msgContextMock.Setup(m => m.SaveChangesAsync(default))
                  .ReturnsAsync(1);
        
        var usrData = new List<User>();
        userContextMock.Setup(u => u.users)
                        .ReturnsDbSet(usrData);

        var msgService = new MessageService(msgContextMock.Object, topicContextMock.Object, userContextMock.Object);

        var response = await msgService.CreateMessage(new Message{topicid=1, userid=1, content="some content"});
        Assert.That(response, Is.TypeOf(typeof(OkObjectResult)));

        msgContextMock.Verify(m => m.messages.Add(It.IsAny<Message>()), Times.Once);
        msgContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);

        topicContextMock.Verify(t=> t.SaveChangesAsync(default), Times.Once);

        var updatedTopic = topicData.FirstOrDefault(t => t.id==1);

        Assert.That(updatedTopic.messagecount, Is.EqualTo(1));
        Assert.That(updatedTopic.lastupdated, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromSeconds(1)));

    }

    /// <summary>
    /// test deleting messages with a specific topic id.
    /// </summary>

    [Test]
    public async Task DeleteMessages_Success()
    {
        var msgContextMock = new Mock<MessageContext>();
        var topicContextMock = new Mock<TopicContext>();
        var userContextMock = new Mock<UserContext>();

        var msgData = new List<Message>
        {
            new Message{ id=1, topicid=1, userid=1, content="some content"},
            new Message{ id=2, topicid=1, userid=2, content="some other content"}
        };

        msgContextMock.Setup(m => m.messages)
                        .ReturnsDbSet(msgData);
        
        msgContextMock.Setup(m => m.SaveChangesAsync(default))
                  .ReturnsAsync(2);

        var msgService = new MessageService(msgContextMock.Object, topicContextMock.Object, userContextMock.Object);

        var response = await msgService.DeleteMessage(1);
        Assert.That(response, Is.TypeOf(typeof(OkObjectResult)));

        msgContextMock.Verify(m => m.messages.RemoveRange(It.IsAny<IEnumerable<Message>>()), Times.Once);
       

    }

    /// <summary>
    /// Test getting all messages for a topic.
    /// </summary>

    [Test]
    public async Task GetAllMessages_Success()
    {
        var msgContextMock = new Mock<MessageContext>();
        var topicContextMock = new Mock<TopicContext>();
        var userContextMock = new Mock<UserContext>();

        var msgData = new List<Message>
        {
            new Message{ id=1, topicid=1, userid=1, content="some content", postdate=DateTime.UtcNow},
            new Message{ id=2, topicid=1, userid=2, content="some other content", postdate=DateTime.UtcNow}
        };

        msgContextMock.Setup(m => m.messages)
                        .ReturnsDbSet(msgData);

        var usrData = new List<User>
        {
            new User{ id=1, username="user1"},
            new User{ id=2, username="user2"}
        };

        userContextMock.Setup(u => u.users)
                        .ReturnsDbSet(usrData);


        var msgService = new MessageService(msgContextMock.Object, topicContextMock.Object, userContextMock.Object);

        var response = await msgService.GetAllMessages(1);
        
        Assert.That(response.Count(), Is.EqualTo(2));
        Assert.That(response[0].content, Is.EqualTo("some content"));
        Assert.That(response[0].username, Is.EqualTo("user1"));
        Assert.That(response[1].content, Is.EqualTo("some other content"));
        Assert.That(response[1].username, Is.EqualTo("user2"));
    }

}