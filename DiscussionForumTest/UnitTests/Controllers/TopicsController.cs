using System.Net;
using System.Net.Http.Json;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

public class TopicController_UnitTests 
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
    /// test creating a topic with too long topicname and unallowed characters.
    /// The requests should fail
    /// </summary>

    [Test]
    public async Task CreateTopic_InvalidTopicname(){

        //name is too long

        var tooLongString = new string('a', 501);

        Topic topic = new Topic (){
            topicname=tooLongString
        };

        var content = JsonConvert.SerializeObject(topic);

        var response = await client.PostAsync("/api/Topics", new StringContent(content, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));

        //name has unallowed characters

        var unallowedCharacters = "topic!!";

        topic.topicname=unallowedCharacters;

        content=JsonConvert.SerializeObject(topic);

        var response2 = await client.PostAsync("/api/Topics", new StringContent(content, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response2.StatusCode));


    }

    [Test]
    public async Task CreateTopic_Success(){


        Topic topic = new Topic (){
            topicname="topic1"
        };

        var content = JsonConvert.SerializeObject(topic);

        content=JsonConvert.SerializeObject(topic);

        var response = await client.PostAsync("/api/Topics", new StringContent(content, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));


    }


}