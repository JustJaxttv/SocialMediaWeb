using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaWeb.Data;
using SocialMediaWeb.Models;

namespace SocialMediaWeb.Areas.ForumArea.Controllers
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
            ViewBag.Forums = _dbContext.Forums?.ToList() ?? new List<Forum>();

            ViewBag.Title = "Browse Forums";

            return View();
        }

        public IActionResult View(int id)
        {
            var forum = _dbContext.Forums
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                .FirstOrDefault(f => f.Id == id);

            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }
    }
}
