public class MessageResponse
{
    public int id { get; set; }
    public string? content { get; set; }
    public int topicid { get; set; }
    public int userid { get; set; }
    public string? username { get; set; }
    public int upvotes { get; set; }
    public DateTime postdate { get; set; } = DateTime.UtcNow;
}