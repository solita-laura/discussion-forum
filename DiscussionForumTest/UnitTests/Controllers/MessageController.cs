using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;


public class MessageController_UnitTests 
{

    [SetUp]
    public void Setup()
    {
    }

    /// <summary>
    /// Test the message controller by passing a negative query value
    /// The request should fail
    /// </summary>

    [Test]
    public async Task GetMessages_NegativeQuery (){

        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/Topics/Message?topicid=-1");
        
        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
    }

    /// <summary>
    /// test the message controller by passing a valid query
    /// the request should succeed
    /// </summary>

    [Test]
    public async Task GetMessages_ValidQuery (){

        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/Topics/Message?topicid=1");
        
        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
    }


    /// <summary>
    /// create a message that is length of 501 (maximum 500).
    /// The request should fail.
    /// </summary>
    
    [Test]
    public async Task CreateMessage_InvalidMessage(){

        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var tooLongString = new string('a', 501);
        var response = await client.PostAsync("/api/Topics/Message", new StringContent(tooLongString, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
    }
}