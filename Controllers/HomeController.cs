using bojan_recipe.Data;
using bojan_recipe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace bojan_recipe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var latestRecipes = _context.Recipe.OrderByDescending(r => r.Id).Take(3).ToList();
            var latestTutorials = _context.Tutorial.OrderByDescending(r => r.TutorialId).Take(3).ToList();
            var latestPhotos = _context.Gallery.OrderByDescending(r => r.GalleryId).Take(8).ToList();

            ViewData["LatestTutorials"] = latestTutorials;
            ViewData["LatestPhotos"] = latestPhotos;

            return View(latestRecipes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}