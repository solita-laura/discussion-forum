using System.Net;
using DiscussionForum.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;


public class MessageController_UnitTests 
{

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetMessages_NegativeQuery (){

        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/Topics/Message?topicid=-1");
        
        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));
    }

    [Test]
    public async Task GetMessages_ValidQuery (){

        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/Topics/Message?topicid=1");
        
        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
    }
}