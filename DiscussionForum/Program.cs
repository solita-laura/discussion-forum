using System.Text;
using DiscussionForum.Models;
using Microsoft.EntityFrameworkCore;
using DiscussionForum.Services;
using DiscussionForum.DbEntities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

//Add cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<MessageService>();

//add dbcontext
builder.Services.AddDbContext<TopicContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<UserContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<MessageContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddIdentity<User, IdentityRole>(options =>{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    //lockout options
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
    .AddEntityFrameworkStores<UserContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => 
    {
        options.Cookie.Name = "authtoken";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope()){
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if(!await roleManager.RoleExistsAsync("User")){
        await roleManager.CreateAsync(new IdentityRole("User"));
    }
    if(!await roleManager.RoleExistsAsync("Admin")){
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { } //for testing purposes