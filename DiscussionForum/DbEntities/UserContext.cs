using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Models;

public class UserContext : IdentityDbContext<User>
{
    public UserContext(){}
    public UserContext(DbContextOptions<UserContext> options) : base(options){}

    public virtual DbSet<User> users {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("roles");
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("userroles");
        });

        modelBuilder.Ignore<IdentityUserToken<string>>();
        modelBuilder.Ignore<IdentityUserLogin<string>>();
        modelBuilder.Ignore<IdentityRoleClaim<string>>();

        modelBuilder.Entity<User>()
            .Ignore(c => c.Email)
            .Ignore(c => c.NormalizedEmail)
            .Ignore(c => c.EmailConfirmed)
            .Ignore(c => c.SecurityStamp)
            .Ignore(c => c.ConcurrencyStamp)
            .Ignore(c => c.PhoneNumber)
            .Ignore(c => c.PhoneNumberConfirmed)
            .Ignore(c => c.TwoFactorEnabled)
            .Ignore(c => c.LockoutEnabled)
            .Ignore(c => c.LockoutEnd)
            .Ignore(c => c.AccessFailedCount);
    }

}

public class User : IdentityUser
{
}



