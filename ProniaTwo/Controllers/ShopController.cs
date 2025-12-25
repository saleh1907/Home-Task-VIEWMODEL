using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaTwo.Context;
using System.Threading.Tasks;

namespace ProniaTwo.Controllers
{
    public class ShopController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Productts.ToListAsync();
            return View(products);
        }
    }
}
