using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Models;

public class TopicContext : DbContext
{
    public TopicContext(DbContextOptions<TopicContext> options):base(options) {}
    public DbSet<Topic> topics {get; set; }
}

public class Topic {

    public int id {get; set; }
    public string? topicname {get; set; }
    public int messagecount {get; set; }
    public DateTime lastupdated {get; set; }
}
