using System.Net;
using DiscussionForum.DbEntities;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;


public class MessageController_UnitTests 
{

    private WebApplicationFactory<Program> factory;
    private HttpClient client;

    [SetUp]
    public void Setup()
    {
        factory = new WebApplicationFactory<Program>();
        client = factory.CreateClient();
    }

    [TearDown]
    public void TearDown(){
        factory.Dispose();
        client.Dispose();
    }

    /// <summary>
    /// Test the message controller by passing a negative query value
    /// The request should fail
    /// </summary>

    [Test]
    public async Task GetMessages_NegativeQuery (){
        var response = await client.GetAsync("/api/Topics/Message?topicid=-1");
        
        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
    }

    /// <summary>
    /// test the message controller by passing a valid query
    /// the request should succeed
    /// </summary>

    [Test]
    public async Task GetMessages_ValidQuery (){

        var response = await client.GetAsync("/api/Topics/Message?topicid=4");
        
        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
    }


    /// <summary>
    /// create a message that is length of 501 (maximum 500).
    /// The request should fail.
    /// </summary>
    
    [Test]
    public async Task CreateMessage_InvalidMessage(){

        var tooLongString = new string('a', 501);

        var message = new Message(){
            content=tooLongString,
            topicid=4
        };

        var content = JsonConvert.SerializeObject(message);

        var response = await client.PostAsync("/api/Topics/Message", new StringContent(content, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
    }

    [Test]
    public async Task CreateMessage_Success(){

        var message = new Message(){
            content="new message",
            topicid=4
        };

        var content = JsonConvert.SerializeObject(message);

        var response = await client.PostAsync("/api/Topics/Message", new StringContent(content, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task UpdateMessage_NotAuthorized(){

        var content = JsonConvert.ToString("modified content");

        var response = await client.PutAsync("/api/Topics/Message?messageid=3", new StringContent(content, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));

    }

}