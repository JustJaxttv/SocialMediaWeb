using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaWeb.Models;

namespace SocialMediaProject.Areas.PostArea.Controllers
{
    [Area("PostArea")]
    public class PostController : Controller
    {
        private readonly DBContext _dbContext;

        public PostController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Create(int forumId)
        {
            ViewBag.ForumId = forumId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Post model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Posts.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("View", "Forum", new { id = model.ForumId, area = "ForumArea" });
            }
            return View(model);
        }

        public IActionResult View(int id)
        {
            var post = _dbContext.Posts
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.Id == id);
            return View(post);
        }
    }
}
