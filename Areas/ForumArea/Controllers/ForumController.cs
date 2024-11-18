using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaWeb.Models;

namespace SocialMediaProject.Areas.ForumArea.Controllers
{
    [Area("ForumArea")]
    public class ForumController : Controller
    {
        private readonly DBContext _dbContext;

        public ForumController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Browsing()
        {
            var forums = _dbContext.Forums.ToList();
            return View(forums);
        }

        public IActionResult View(int id)
        {
            var forum = _dbContext.Forums
                .Include(f => f.Posts)
                .FirstOrDefault(f => f.Id == id);
            return View(forum);
        }
    }
}
