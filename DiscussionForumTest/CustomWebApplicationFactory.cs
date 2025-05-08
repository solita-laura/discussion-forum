
using System;
using DiscussionForum.DbEntities;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // Dedicated Test database
    private readonly string _connectionString = "Server=localhost;Port=5000;Username=admin1;Password=admin1;Database=discussion-forum-test;";
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<TopicContext>>();
            services.RemoveAll<DbContextOptions<MessageContext>>();
            services.RemoveAll<DbContextOptions<UserContext>>();

            services.AddDbContext<TopicContext>(options =>
            {
                options.UseNpgsql(_connectionString);
            });

            services.AddDbContext<UserContext>(options =>
            {
                options.UseNpgsql(_connectionString);
            });

            services.AddDbContext<MessageContext>(options =>
            {
                options.UseNpgsql(_connectionString);
            });
        });

        builder.UseEnvironment("Development");
    }
}