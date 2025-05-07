using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

public class AuthController_UnitTests
{

    private WebApplicationFactory<Program> factory;
    private HttpClient client;

    [SetUp]
    public void Setup(){
        factory = new WebApplicationFactory<Program>();
        client = factory.CreateClient();
    }

    [TearDown]
    public void TearDown(){
        factory.Dispose();
        client.Dispose();
    }

    /// <summary>
    /// Test login method with correct username and invalid password and invalid username and correct password.
    /// The test should return BadRequest
    /// </summary>
    /// <returns>BadRequest</returns>
    
    [Test]
    public async Task LoginUser_InvalidPasswordOrUsername(){

        //invalid password
        var registrationRequest = new RegistrationRequest () {Username="user1", Password="invalidpassword"};
        var payload = JsonConvert.SerializeObject(registrationRequest);

        var response = await client.PostAsync("/api/Auth/login-user", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));

        //invalid username
        registrationRequest = new RegistrationRequest () {Username="invalidusername", Password="Password1234!"};
        payload = JsonConvert.SerializeObject(registrationRequest);

        response = await client.PostAsync("/api/Auth/login-user", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));

    }

    /// <summary>
    /// Test a succesfull login with a correct username and password.
    /// The test should return OK
    /// </summary>
    /// <returns>OK</returns>

    [Test]
    public async Task LoginUser_Success(){

        var registrationRequest = new RegistrationRequest () {Username="user1", Password="Password1234!"};
        var payload = JsonConvert.SerializeObject(registrationRequest);

        var response = await client.PostAsync("/api/Auth/login-user", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));

    }

    /// <summary>
    /// Test registering a user with a username that is already registered.
    /// The test should return a statuscode 409
    /// </summary>
    /// <returns>Conflict</returns>

    [Test]
    public async Task RegisterUser_UsernameDuplicate (){

        var registrationRequest = new RegistrationRequest () {Username="user1", Password="TESTpassword1234!"};
        var payload = JsonConvert.SerializeObject(registrationRequest);

        var response = await client.PostAsync("/api/Auth/register-user", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.Conflict, Is.EqualTo(response.StatusCode));

    }

    /// <summary>
    /// Test registering a user with a password that doesnt fullfill the password rules.
    /// The test should return BadRequest
    /// </summary>
    /// <returns>BadRequest</returns>

    [Test]
    public async Task RegisterUser_InvalidPassword (){

        //only uppercase letters
        var registrationRequest = new RegistrationRequest () {Username="user1", Password="TESTPASSWORD"};
        var payload = JsonConvert.SerializeObject(registrationRequest);

        var response = await client.PostAsync("/api/Auth/register-user", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));

        //only upper and lower case letters
        registrationRequest = new RegistrationRequest () {Username="user1", Password="TESTpassword"};
        payload = JsonConvert.SerializeObject(registrationRequest);

        response = await client.PostAsync("/api/Auth/register-user", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));

        //only uppercase and lowercase letters and numbers
        registrationRequest = new RegistrationRequest () {Username="user1", Password="TESTpassword1234"};
        payload = JsonConvert.SerializeObject(registrationRequest);

        response = await client.PostAsync("/api/Auth/register-user", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));

        //too short
        registrationRequest = new RegistrationRequest () {Username="user1", Password="tE1!"};
        payload = JsonConvert.SerializeObject(registrationRequest);

        response = await client.PostAsync("/api/Auth/register-user", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.BadRequest, Is.EqualTo(response.StatusCode));

    }

    /// <summary>
    /// Test a succesfull user login.
    /// The test should return OK.
    /// </summary>
    /// <returns>OK</returns>

    [Test]
    public async Task RegisterUser_Success (){

        var registrationRequest = new RegistrationRequest () {Username="user18946399", Password="TESTpassword1234!"};
        var payload = JsonConvert.SerializeObject(registrationRequest);

        var response = await client.PostAsync("/api/Auth/register-user", new StringContent(payload, System.Text.Encoding.UTF8, "application/json"));

        Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));

    }
}