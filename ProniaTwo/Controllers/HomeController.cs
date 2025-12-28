using Microsoft.AspNetCore.Mvc;
using ProniaTwo.Context;

namespace ProniaTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
           _context = context; 
        }

        public IActionResult Index()
        {
            var shippings = _context.Shippings.ToList();
           
            ViewBag.Shippings = shippings;

            return View(shippings);
        }
    }
}
