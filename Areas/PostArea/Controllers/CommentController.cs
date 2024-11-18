using Microsoft.AspNetCore.Mvc;
using SocialMediaWeb.Models;

namespace SocialMediaProject.Areas.PostArea.Controllers
{
    [Area("PostArea")]
    public class CommentController : Controller
    {
        private readonly DBContext _dbContext;

        public CommentController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Create(Comment model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Comments.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("View", "Post", new { id = model.PostId, area = "PostArea" });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            if (TempData["UserId"] is int userId && comment.UserId == userId)
            {
                _dbContext.Comments.Remove(comment);
                _dbContext.SaveChanges();

                return RedirectToAction("View", "Post", new { id = comment.PostId, area = "PostArea" });
            }

            return Forbid();
        }
    }
}