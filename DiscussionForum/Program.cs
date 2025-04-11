using System.Text;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DiscussionForum.Services;
using Microsoft.Extensions.Options;
using DiscussionForum.DbEntities;

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
builder.Services.AddScoped<UserService>();

//add dbcontext
builder.Services.AddDbContext<TopicContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<UserContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<MessageContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

//create jwt bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:ValidIssuer"],
        ValidAudience = configuration["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var cookie = context.Request.Cookies["token"];
            if (cookie!=null)
            {
                context.Token = cookie;
            }
            return Task.CompletedTask;
        }
    };
    });


var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();