using DiscussionForum.Models;
using DiscussionForum.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;

namespace DiscussionForumTest;

public class UserService_UnitTests
{

    UserService userService;

    private readonly UserContext _context;
    private readonly IConfiguration _config;


    [SetUp]
    public void Setup()
    {
        userService = new UserService(_config, _context);

    }

    [Test]
    public void CreateSalt()
    {
        Assert.IsNotNull(userService.CreateSalt(16));
        Assert.That(16.Equals(userService.CreateSalt(16).Length));
    }

    [Test]
    public void CreateKey()
    {
        Assert.IsNotNull(userService.CreateKey("testpassword", userService.CreateSalt(16)));
        Assert.That(16.Equals(userService.CreateKey("testpwd", userService.CreateSalt(16)).Length));
    }

    [Test]
    public async Task EncryptPassword()
    {
        Assert.IsNotNull(userService.EncryptPassword("testpassword", userService.CreateSalt(16)));
        Assert.That(44.Equals(userService.EncryptPassword("testpassword", userService.CreateSalt(16)).ToString().Length));
    }

    [Test]
    public async Task DecryptPassword()
    {
        byte[] salt = userService.CreateSalt(16);
        string password = "testpassword";
        var encryptedPassword = await userService.EncryptPassword(password, salt);
        var decryptedPassword = await userService.DecryptPassword(encryptedPassword, salt, password);

        Assert.That(password.Equals(decryptedPassword));
    }

    [Test]
    public async Task Login_UserNotFound()
    {
        var data = new List<User>
        {
            new User { id = 1, username = "testuser", password = "password", salt = "testsalt" }
        };

        var userContextMock = new Mock<UserContext>();
        userContextMock.Setup(us => us.users)
                        .ReturnsDbSet(data);

        UserService _userService = new UserService(_config, userContextMock.Object);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await _userService.Login(new LoginRequest { Username = "nonexisting", Password = "password" })
        );

        Assert.That(ex.Message, Is.EqualTo("Login request failed."));_userService = new UserService(_config, userContextMock.Object);
    }

    [Test]
    public async Task Login_WrongPassword()
    {
        UserService userService = new UserService(_config, _context);
        
        byte[] salt = userService.CreateSalt(16);
        byte[] hashedPassword = await userService.EncryptPassword("testpassword", salt);
        
        var data = new List<User>
        {
            new User { id = 1, username = "testuser", password = Convert.ToBase64String(hashedPassword), salt = Convert.ToBase64String(salt) }
        };

        var userContextMock = new Mock<UserContext>();
        userContextMock.Setup(us => us.users)
                        .ReturnsDbSet(data);
        UserService _userService = new UserService(_config, userContextMock.Object);

        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await _userService.Login(new LoginRequest { Username = "testuser", Password = "wrongpassword" })
        );
        Assert.That(ex.Message, Is.EqualTo("Login request failed."));

    }

    [Test]
    public async Task Login_Success()
    {
        UserService userService = new UserService(_config, _context);
        
        byte[] salt = userService.CreateSalt(16);
        byte[] hashedPassword = await userService.EncryptPassword("testpassword", salt);
        
        var data = new List<User>
        {
            new User { id = 1, username = "testuser", password = Convert.ToBase64String(hashedPassword), salt = Convert.ToBase64String(salt) }
        };

        var userContextMock = new Mock<UserContext>();
        userContextMock.Setup(us => us.users)
                        .ReturnsDbSet(data);

        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Jwt:Secret"]).Returns("testsecretkey");
        configMock.Setup(c => c["Jwt:ValidIssuer"]).Returns("testissuer");
        configMock.Setup(c => c["Jwt:ValidAudience"]).Returns("testaudience");

        UserService _userService = new UserService(configMock.Object, userContextMock.Object);

        var token = await _userService.Login(new LoginRequest { Username = "testuser", Password = "testpassword" });
        Assert.That(token, Is.Not.Null);

    }
}