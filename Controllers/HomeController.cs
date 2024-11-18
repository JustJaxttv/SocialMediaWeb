using Microsoft.AspNetCore.Mvc;
using SocialMediaWeb.Data;
using SocialMediaWeb.Models;

namespace SocialMediaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBContext _dbContext;

        public HomeController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var recentPosts = _dbContext.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToList();

            var popularForums = _dbContext.Forums
                .OrderByDescending(f => f.Posts.Count)
                .Take(5)
                .ToList();

            ViewBag.RecentPosts = recentPosts ?? new List<Post>();
            ViewBag.PopularForums = popularForums ?? new List<Forum>();

            return View();
        }
    }
}
