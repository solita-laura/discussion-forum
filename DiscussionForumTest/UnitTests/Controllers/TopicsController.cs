using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

public class TopicController_UnitTests 
{

    [SetUp]
    public void Setup()
    {
    }

    /// <summary>
    /// test creating a topic with too long topicname and unallowed characters.
    /// The requests should fail
    /// </summary>

    [Test]
    public async Task CreateTopic_InvalidTopicname(){

        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var tooLongString = new string('a', 501);
        var response = await client.PostAsync("/api/Topics", new StringContent(tooLongString, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));

        var unallowedCharacters = "topic!!";

        var response2 = await client.PostAsync("/api/Topics", new StringContent(unallowedCharacters, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response2.StatusCode));


    }
}