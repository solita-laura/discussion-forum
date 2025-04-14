using System;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.DbEntities;

public class MessageContext : DbContext
{

    public MessageContext(DbContextOptions<MessageContext> options) : base(options) { }
    public MessageContext() { }

    public virtual DbSet<Message> messages { get; set; }

}

public class Message
{
    public int id { get; set; }
    public string? content { get; set; }
    public int topicid { get; set; }
    public int userid { get; set; }
    public int upvotes { get; set; }
}
