using System;

namespace DiscussionForum.Models;

public class LoginRequest
{
    public string? Email {get; set;}
    public string? Password{get; set;}
}
