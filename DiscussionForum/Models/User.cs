using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DiscussionForum.Models;

public class User
{
    public string? Id {get; set;}
    public string? username {get; set;}
    public string? password {get; set;}
    public byte[]? salt {get; set;}

}
