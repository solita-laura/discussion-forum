using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Models;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options){}

    public DbSet<User> users {get; set;}

}

