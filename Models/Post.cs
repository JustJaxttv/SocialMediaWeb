namespace SocialMediaWeb.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int ForumId { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
        public Forum Forum { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
