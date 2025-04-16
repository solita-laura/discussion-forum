using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Models;

public class TopicContext : DbContext
{
    public TopicContext(DbContextOptions<TopicContext> options):base(options) {}

    public TopicContext(){}
    public virtual DbSet<Topic> topics {get; set; }


}

public class Topic {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id {get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Topic name can only contain letters, numbers, and spaces.")]
    [StringLength(20, ErrorMessage = "Topic name cannot be longer than 20 characters.")]
    public string? topicname {get; set; }
    public int messagecount {get; set; }
    public DateTime? lastupdated {get; set; } = DateTime.UtcNow;
}
