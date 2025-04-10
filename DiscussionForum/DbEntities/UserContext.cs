using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Models;

public class UserContext : DbContext
{
    public UserContext(){}
    public UserContext(DbContextOptions<UserContext> options) : base(options){}

    public virtual DbSet<User> users {get; set;}

}

public class User
{
    public int? id {get; set;}
    public string? username {get; set;}
    public string? password {get; set;}
    public string? salt {get; set;}

}



