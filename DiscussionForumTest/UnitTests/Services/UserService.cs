using DiscussionForum.Models;
using DiscussionForum.Services;
using Microsoft.Extensions.Configuration;

namespace DiscussionForumTest;

public class UserService_UnitTests
{

    UserService _userService;

    private readonly UserContext _context;
    private readonly IConfiguration _config;


    [SetUp]
    public void Setup()
    {
        _userService = new UserService(_config, _context);
    }

    [Test]
    public void CreateSalt()
    {
        Assert.IsNotNull(_userService.CreateSalt(16));
        Assert.That(16.Equals(_userService.CreateSalt(16).Length));
    }

    [Test]
    public void CreateKey()
    {
        Assert.IsNotNull(_userService.CreateKey("testpassword", _userService.CreateSalt(16)));
        Assert.That(16.Equals(_userService.CreateKey("testpwd", _userService.CreateSalt(16)).Length));
    }

    [Test]
    public void EncryptPassword()
    {
        Assert.IsNotNull(_userService.EncryptPassword("testpassword", _userService.CreateSalt(16)));
        Assert.That(44.Equals(_userService.EncryptPassword("testpassword", _userService.CreateSalt(16)).ToString().Length));
    }

    [Test]
    public async Task DecryptPassword()
    {
        byte[] salt = _userService.CreateSalt(16);
        string password = "testpassword";
        var encryptedPassword = await _userService.EncryptPassword(password, salt);
        var decryptedPassword = await _userService.DecryptPassword(encryptedPassword, salt, password);

        Assert.That(password.Equals(decryptedPassword));
    }
}