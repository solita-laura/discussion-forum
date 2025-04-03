using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Models;

public class TopicContext : DbContext
{
    public TopicContext(DbContextOptions<TopicContext> options):base(options) {}
    public DbSet<Topic> Topics {get; set; }
}

public class Topic {

    public int Id {get; set; }
    public string? topicName {get; set; }
    public int messageCount {get; set; }
    public DateTime lastUpdated {get; set; }
}
