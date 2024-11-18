using Microsoft.EntityFrameworkCore;
using SocialMediaWeb.Models;

namespace SocialMediaWeb.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Forum> Forums { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Forum)
                .WithMany(f => f.Posts)
                .HasForeignKey(p => p.ForumId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "User1", Email = "user1@example.com", PasswordHash = "password1", CreatedAt = DateTime.UtcNow },
                new User { Id = 2, Username = "User2", Email = "user2@example.com", PasswordHash = "password2", CreatedAt = DateTime.UtcNow }
            );

            modelBuilder.Entity<Forum>().HasData(
                new Forum { Id = 1, Title = "General Discussion", Description = "Talk about anything here", CreatedAt = DateTime.UtcNow },
                new Forum { Id = 2, Title = "Tech News", Description = "Discuss the latest in technology", CreatedAt = DateTime.UtcNow }
            );

            modelBuilder.Entity<Post>().HasData(
                new Post { Id = 1, Title = "First Post", Content = "Welcome to the General Discussion forum!", UserId = 1, ForumId = 1, CreatedAt = DateTime.UtcNow },
                new Post { Id = 2, Title = "Tech Innovations 2024", Content = "Let's discuss new tech trends for 2024!", UserId = 2, ForumId = 2, CreatedAt = DateTime.UtcNow }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment { Id = 1, Content = "Great post!", UserId = 2, PostId = 1 },
                new Comment { Id = 2, Content = "Looking forward to this discussion!", UserId = 1, PostId = 2 }
            );
        }
    }
}