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
            var shippings = _context.Shippings
                                   //.Where(s => s.Title == "Pulsuz Catdirilma" ||
                                   //            s.Title == "tehlikesiz odenis" ||
                                   //            s.Title == "En yaxsi Servis")
                                   .ToList();
           
            ViewBag.Shippings = shippings;

            return View(shippings);
        }
    }
}
