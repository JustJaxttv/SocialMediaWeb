using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaWeb.Data;
using SocialMediaWeb.Models;

namespace SocialMediaWeb.Areas.UserArea.Controllers
{
    [Area("UserArea")]
    public class UserController : Controller
    {
        private readonly DBContext _dbContext;

        public UserController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Users.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string usernameOrEmail, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u =>
                (u.Username == usernameOrEmail || u.Email == usernameOrEmail) && u.PasswordHash == password);

            if (user != null)
            {
                TempData["UserId"] = user.Id;
                return RedirectToAction("Profile");
            }

            return View();
        }

        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Profile()
        {
            if (TempData["UserId"] is int userId)
            {
                var user = await _dbContext.Users
                    .Include(u => u.Posts)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Login");
        }
    }
}
