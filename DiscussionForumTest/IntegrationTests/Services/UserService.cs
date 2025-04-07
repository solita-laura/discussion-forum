using System.Threading.Tasks;
using DiscussionForum.Models;
using DiscussionForum.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DiscussionForumTest.IntegrationTests.Services
{
    public class UserService_IntegrationTests
    {
        UserService _userService;

        private ServiceProvider _serviceProvider;
        UserContext _context;
        private readonly IConfiguration _config;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<UserContext>(options=>
                options.UseInMemoryDatabase("UserTestDB"));
            
            _serviceProvider= services.BuildServiceProvider();
            
        }

        [Test]
        public async Task Login()
        {
            byte[] salt = _userService.CreateSalt(16);
            var hashPass = await _userService.EncryptPassword("testpassword", salt);

            var db = _serviceProvider.GetRequiredService<UserContext>();

            db.users.Add(new User{id=1, username="test1", password=Convert.ToBase64String(hashPass), salt=Convert.ToBase64String(salt)});
            db.SaveChanges();

            Assert.Pass(await _userService.Login(new LoginRequest{Username="test1", Password="testpassword"}));
        }
    }

}