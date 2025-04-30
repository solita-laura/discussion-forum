using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace DiscussionForum.DbEntities;

public class MessageContext : DbContext
{

    public MessageContext(DbContextOptions<MessageContext> options) : base(options) { 

    }
    public MessageContext() { }

    public virtual DbSet<Message> messages { get; set; }
}

public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [Required]
    [StringLength(500, ErrorMessage="Maximum length for the content is 500 characters.")]
    public string? content { get; set; }

    [Required]
    public int topicid { get; set; }

    public string? userid { get; set; }

    public int upvotes { get; set; }

    public DateTime postdate {get; set;}
}
